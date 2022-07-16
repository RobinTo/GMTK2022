using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MinerModule : MonoBehaviour, IUpgradable
{
  enum MovingState
  {
    Idle,
    Mining,
    Retrieving,
  }

  [SerializeField]
  LineRenderer mineLine;
  [SerializeField]
  SpriteRenderer grab;
  [SerializeField]
  Sprite grabOpenSprite;
  [SerializeField]
  Sprite grabClosedSprite;

  [SerializeField]
  GameObject scrollingResourcePrefab;

  public int chanceToMine = 5;
  public float timeBetweenMines = 1f;
  public float timer = 0;

  Transform target;
  Transform uiCanvas;
  Camera mainCamera;

  MovingState moving = MovingState.Idle;

  [SerializeField]
  float speedModifier = .5f;
  [SerializeField]
  float maxDistance = 4;

  int level = 0;
  List<Resource> availableResource;
  int maxResourcePerMine = 4;
  [SerializeField]
  AnimationCurve resourceDistribution;

  [SerializeField]
  List<UpgradeCosts> baseUpgradeCosts;

  // Start is called before the first frame update
  void Start()
  {
    mainCamera = Camera.main;
    uiCanvas = GameObject.FindGameObjectWithTag("UICanvas").transform;
    mineLine.SetPositions(new Vector3[] { transform.position, transform.position + Vector3.up + Vector3.right * .25f, transform.position + Vector3.right * .5f });
    grab.transform.position = transform.position + Vector3.right * .5f;

    availableResource = new List<Resource>() { Resource.Wood };
  }

  Vector3 TargetPositionWithOffset()
  {
    return target.position + Vector3.up * .43f;
  }

  // Update is called once per frame
  void Update()
  {
    if (moving == MovingState.Mining)
    {
      Vector3 midPosition = ((target.position + transform.position) / 2) + Vector3.up;
      mineLine.SetPositions(new Vector3[] { transform.position, Vector3.MoveTowards(mineLine.GetPosition(1), midPosition, Time.deltaTime * speedModifier), Vector3.MoveTowards(mineLine.GetPosition(2), TargetPositionWithOffset(), Time.deltaTime * speedModifier) });
      grab.transform.position = mineLine.GetPosition(2);
      float distance = Vector3.Distance(grab.transform.position, TargetPositionWithOffset());
      if (distance < .1f)
      {
        moving = MovingState.Retrieving;
        grab.sprite = grabClosedSprite;
        target.parent = grab.transform;
        target.GetComponent<SpriteRenderer>().sortingOrder = 2;
      }
      else if (distance > maxDistance)
      {
        moving = MovingState.Retrieving;
        grab.sprite = grabOpenSprite;
      }
    }
    if (moving == MovingState.Retrieving)
    {

      Vector3 midPosition = transform.position + Vector3.up;
      mineLine.SetPositions(new Vector3[] { transform.position, Vector3.MoveTowards(mineLine.GetPosition(1), midPosition, Time.deltaTime * speedModifier), Vector3.MoveTowards(mineLine.GetPosition(2), transform.position, Time.deltaTime * speedModifier) });
      grab.transform.position = mineLine.GetPosition(2);

      if (Vector3.Distance(grab.transform.position, transform.position) < .1f)
      {
        moving = MovingState.Idle;
        grab.sprite = grabOpenSprite;
        Destroy(target.gameObject);
        // Get a random resource from availableResources
        int resourceIndex = Random.Range(0, availableResource.Count);
        ResourceManager.instance.ModifyResource(availableResource[resourceIndex], 5);
        ShowScrollingResourcePrefab(availableResource[resourceIndex], 5);
      }
    }
  }

  void ShowScrollingResourcePrefab(Resource resource, int amount)
  {
    Vector3 position = mainCamera.WorldToScreenPoint(transform.position + Vector3.up * .5f);
    ImageAndTextUIElement scrollingResource = ObjectPooler.instance.GetPooledObject(scrollingResourcePrefab, position, Quaternion.identity, uiCanvas).GetComponent<ImageAndTextUIElement>();
    scrollingResource.SetImage(ResourceManager.GetSprite(resource));
    scrollingResource.SetText((amount >= 0 ? "+" : "-") + amount.ToString());
    scrollingResource.GetComponent<TimedPoolObject>().ReturnAfter(scrollingResourcePrefab, 2f);
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other && other.gameObject && other.gameObject.CompareTag("Asteroid"))
    {
      target = other.transform;
      moving = MovingState.Mining;
      grab.sprite = grabOpenSprite;
    }
  }

  public void Upgrade()
  {
    level++;
    maxResourcePerMine++;

    if (level == 1)
    {
      availableResource.Add(Resource.Iron);
    }
    if (level == 2)
    {
      availableResource.Add(Resource.Gold);
    }
    if (level == 3)
    {
      availableResource.Add(Resource.Diamond);
    }
  }

  public List<ResourceCost> GetCost()
  {
    if (baseUpgradeCosts.Count > level)
    {
      return baseUpgradeCosts[level].costs;
    }
    else
    {
      List<ResourceCost> cost = new List<ResourceCost>();
      List<ResourceCost> lastResourceCost = baseUpgradeCosts[baseUpgradeCosts.Count - 1].costs;
      for (int i = 0; i < lastResourceCost.Count; i++)
      {
        cost.Add(new ResourceCost(lastResourceCost[i].resource, lastResourceCost[i].amount * level));
      }
      return cost;
    }

  }
}
