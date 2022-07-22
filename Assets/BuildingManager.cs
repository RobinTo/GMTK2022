using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingId
{
  Connection = 0,
  Shield = 1,
  Rocket = 2,
  Miner = 3,
  Repair = 4,
  SpaceShipBrain = 5,
  CircuitProducer = 6,
  AdvancedCircuitProducer = 7,
  HomingRocket = 8,
  PassiveMiner = 9
}

[System.Serializable]
public struct BuildingBuyButton
{
  public BuildingId buildingId;
  public GameObject button;
  public Transform costContainer;
  public List<ResourceCost> cost;
  public GameObject prefab;

  public BuildingBuyButton(BuildingId buildingId, GameObject button, Transform costContainer, List<ResourceCost> cost, GameObject prefab)
  {
    this.buildingId = buildingId;
    this.button = button;
    this.costContainer = costContainer;
    this.cost = cost;
    this.prefab = prefab;
  }
}

public class BuildingManager : MonoBehaviour
{
  public static BuildingManager instance;
  [SerializeField]
  SpriteRenderer preview;
  BuildingPreview buildingPreview;

  [SerializeField]
  LayerMask spaceshipLayer;
  [SerializeField]
  LineRenderer lineRenderer;

  [SerializeField]
  float maxConnectionDistance = 4;

  [SerializeField]
  public List<BuildingBuyButton> buildingBuyButtons;
  [SerializeField]
  ImageAndTextUIElement buildingCostPrefab;

  public List<BuildingId> UnlockedBuildings;

  bool building = false;
  BuildingId buildingBuildingId;
  GameObject buildingObject;
  List<ResourceCost> buildingCost;

  bool buildingConnection = false;
  public bool IsbuildingConnection { get { return buildingConnection; } }
  SpaceShipModule moduleA;

  Camera mainCamera;

  List<BuildingId> freeBuildings = new List<BuildingId>();

  public Action<GameObject> OnBuildingBuilt;

  void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }
    else
    {
      Destroy(gameObject);
    }
  }

  void Start()
  {
    mainCamera = Camera.main;
    buildingPreview = preview.GetComponent<BuildingPreview>();
    preview.gameObject.SetActive(false);
    UpdateUnlockedBuildings();
  }

  void Update()
  {
    if (buildingConnection)
    {
      if (Input.GetMouseButtonDown(1))
      {
        buildingConnection = false;
        lineRenderer.enabled = false;
        moduleA = null;
      }
      if (moduleA != null)
      {

        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        lineRenderer.SetPositions(new Vector3[] {
              moduleA.transform.position,
              mousePosition
          });
      }
    }

    if (!building) return;

    Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    preview.transform.position = new Vector3(worldPoint.x, worldPoint.y, 0);

    // Check if any 2d collider overlaps on the spaceshipLayer layermask
    Collider2D collision = Physics2D.OverlapCircle(preview.transform.position, 0.5f, spaceshipLayer);
    bool isValid = collision == null && buildingPreview.GetDistanceToClosestSpaceShipModule() <= maxConnectionDistance;

    if (!isValid)
    {
      preview.color = Color.red;
    }
    else
    {
      preview.color = Color.green;
    }

    if (Input.GetMouseButtonDown(0))
    {
      if (isValid)
      {
        GameObject newBuilding = Instantiate(buildingObject, preview.transform.position, Quaternion.identity);
        OnBuildingBuilt?.Invoke(newBuilding);
        if (!freeBuildings.Contains(buildingBuildingId))
        {
          ResourceManager.instance.SpendResources(buildingCost);
        }
        else
        {
          freeBuildings.Remove(buildingBuildingId);
        }
        preview.gameObject.SetActive(false);
        building = false;
      }
    }
    if (Input.GetMouseButtonDown(1))
    {
      preview.gameObject.SetActive(false);
      building = false;
    }
  }

  public void StartBuild(GameObject gameObject)
  {
    SpaceShipModule module = gameObject.GetComponent<SpaceShipModule>();
    BuildingBuyButton toBuild = buildingBuyButtons.Find(x => x.buildingId == module.BuildingId);

    if (!ResourceManager.instance.CanAfford(toBuild.cost) && !freeBuildings.Contains(module.BuildingId))
    {
      return;
    }
    buildingBuildingId = module.BuildingId;
    preview.gameObject.SetActive(true);
    building = true;
    buildingObject = toBuild.prefab;
    buildingCost = toBuild.cost;
  }

  public void StartBuildingConnection()
  {
    BuildingBuyButton toBuild = buildingBuyButtons.Find(x => x.buildingId == BuildingId.Connection);
    if (ResourceManager.instance.CanAfford(toBuild.cost))
    {
      buildingBuildingId = BuildingId.Connection;
      buildingCost = toBuild.cost;
      buildingConnection = true;
      moduleA = null;
    }
  }

  public void ModuleClicked(SpaceShipModule module)
  {
    if (moduleA == null && !module.HasReachedMaxConnections())
    {
      moduleA = module;
      lineRenderer.enabled = true;
      lineRenderer.SetPositions(new Vector3[] {
              moduleA.transform.position,
              mainCamera.ScreenToWorldPoint(Input.mousePosition)
          });
    }
    else if (moduleA != null)
    {
      moduleA.CreateModuleConnection(module);
      buildingConnection = false;
      lineRenderer.enabled = false;

      ResourceManager.instance.SpendResources(buildingCost);
    }
  }

  public void UnlockBuilding(BuildingId id)
  {
    UnlockedBuildings.Add(id);
    UpdateUnlockedBuildings();
  }

  public void UpdateUnlockedBuildings()
  {
    foreach (BuildingBuyButton btn in buildingBuyButtons)
    {
      btn.button.SetActive(UnlockedBuildings.Contains(btn.buildingId));

      for (int i = btn.costContainer.childCount - 1; i >= 0; i--)
      {
        Destroy(btn.costContainer.GetChild(i).gameObject);
      }

      foreach (ResourceCost cost in btn.cost)
      {
        ImageAndTextUIElement element = Instantiate(buildingCostPrefab, btn.costContainer);
        element.SetText(cost.amount.ToString());
        element.SetImage(ResourceManager.GetSprite(cost.resource));
      }
    }
  }

  public void SetFree(BuildingId buildingId)
  {
    freeBuildings.Add(buildingId);
  }
}
