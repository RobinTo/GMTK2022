using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MinerModule : MonoBehaviour, IUpgradable
{
  public enum MovingState
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

  public Transform target;
  public Asteroid targetAsteroid;
  Transform uiCanvas;
  Camera mainCamera;

  public MovingState moving = MovingState.Idle;

  [SerializeField]
  float speedModifier = .5f;

  int level = 0;
  List<Resource> availableResource;
  int maxResourcePerMine = 4;
  [SerializeField]
  AnimationCurve resourceDistribution;

  [SerializeField]
  List<UpgradeCosts> baseUpgradeCosts;

  List<Transform> objectsInRange;

  // Start is called before the first frame update
  void Start()
  {
    objectsInRange = new List<Transform>();
    mainCamera = Camera.main;
    uiCanvas = GameObject.FindGameObjectWithTag("UIHealthbarParent").transform;
    mineLine.SetPositions(new Vector3[] { transform.position, transform.position + Vector3.up + Vector3.right * .25f, transform.position + Vector3.right * .5f });
    grab.transform.position = transform.position + Vector3.right * .5f;

    availableResource = new List<Resource>() { Resource.Wood };

    SpaceShipModule module = transform.root.GetComponent<SpaceShipModule>();
    Debug.Log("Subscribing to destruction of " + module.name);
    module.OnModuleDestroyed += UnparentAsteroidTarget;
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
      if (target == null || targetAsteroid == null || targetAsteroid.IsGrabbed)
      {
        target = null;
        targetAsteroid = null;
        moving = MovingState.Retrieving;
        LookForNewTarget();
        return;
      }
      Vector3 midPosition = ((target.position + transform.position) / 2) + Vector3.up;
      mineLine.SetPositions(new Vector3[] { transform.position, Vector3.MoveTowards(mineLine.GetPosition(1), midPosition, Time.deltaTime * speedModifier), Vector3.MoveTowards(mineLine.GetPosition(2), TargetPositionWithOffset(), Time.deltaTime * speedModifier) });
      grab.transform.position = mineLine.GetPosition(2);
      float distance = Vector3.Distance(grab.transform.position, TargetPositionWithOffset());
      if (distance < .1f)
      {
        Asteroid asteroid = target.GetComponent<Asteroid>();
        if (asteroid.IsGrabbed)
        {
          moving = MovingState.Retrieving;
          grab.sprite = grabOpenSprite;
          target = null;
          targetAsteroid = null;
          LookForNewTarget();
          return;
        }
        moving = MovingState.Retrieving;
        grab.sprite = grabClosedSprite;
        target.parent = grab.transform;
        asteroid.IsGrabbed = true;
        asteroid.enabled = false;
        target.GetComponent<SpriteRenderer>().sortingOrder = 2;
      }
    }
    else if (moving == MovingState.Retrieving)
    {

      Vector3 midPosition = transform.position + Vector3.up;
      mineLine.SetPositions(new Vector3[] { transform.position, Vector3.MoveTowards(mineLine.GetPosition(1), midPosition, Time.deltaTime * speedModifier), Vector3.MoveTowards(mineLine.GetPosition(2), transform.position, Time.deltaTime * speedModifier) });
      grab.transform.position = mineLine.GetPosition(2);

      if (Vector3.Distance(grab.transform.position, transform.position) < .1f)
      {
        moving = MovingState.Idle;
        grab.sprite = grabOpenSprite;
        if (target != null)
        {
          if (targetAsteroid != null)
          {
            foreach (ResourceCost r in targetAsteroid.Resources)
            {
              ResourceManager.instance.ModifyResource(r.resource, r.amount);
              ShowScrollingResourcePrefab(r.resource, r.amount);
            }
          }
          target.GetComponent<TimedPoolObject>().ReturnNow();
        }
        LookForNewTarget();
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
      objectsInRange.Add(other.transform);
      LookForNewTarget();
    }
  }

  void OnTriggerExit2D(Collider2D other)
  {
    if (other && other.gameObject && other.gameObject.CompareTag("Asteroid"))
    {
      if (other == null)
      {
        // Remove all null values from objectsInRange
        Debug.Log("Remoing null values from objectsInRange");
        objectsInRange.RemoveAll(x => x == null);
        return;
      }

      objectsInRange.Remove(other.transform);

      if (other.transform == target && other.transform.parent != grab.transform)
      {
        moving = MovingState.Retrieving;
        grab.sprite = grabOpenSprite;
        target = null;
        LookForNewTarget();
      }
    }
  }

  void LookForNewTarget()
  {
    if (moving == MovingState.Retrieving && target != null)
    {
      return;
    }
    // Find nearest target from objectsInRange
    if (objectsInRange.Count > 0)
    {
      Transform nearest = null;
      Asteroid nearestAsteroid = null;
      float nearestDistance = Mathf.Infinity;
      foreach (Transform t in objectsInRange)
      {
        Asteroid tasteroid = t.GetComponent<Asteroid>();
        if (tasteroid.IsGrabbed)
        {
          continue;
        }
        float distance = Vector3.Distance(grab.transform.position, t.position);
        if (distance < nearestDistance)
        {
          nearest = t;
          nearestAsteroid = tasteroid;
          nearestDistance = distance;
        }
      }

      if (nearest == null)
      {
        target = null;
        targetAsteroid = null;
        moving = MovingState.Retrieving;
      }
      else
      {
        target = nearest;
        targetAsteroid = nearestAsteroid;
        moving = MovingState.Mining;
        grab.sprite = grabOpenSprite;
      }
    }
    else
    {
      target = null;
      targetAsteroid = null;
      moving = MovingState.Retrieving;
    }
  }

  public void Upgrade()
  {
    level++;
    maxResourcePerMine++;
    speedModifier *= 2;

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

  void UnparentAsteroidTarget(SpaceShipModule module)
  {
    Debug.Log("Trying to clear parent on destruction");
    for (int i = grab.transform.childCount - 1; i >= 0; i--)
    {
      Transform child = grab.transform.GetChild(i);
      child.SetParent(null);
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
