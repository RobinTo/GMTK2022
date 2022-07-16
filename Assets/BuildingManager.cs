using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
  public static BuildingManager instance;
  [SerializeField]
  SpriteRenderer preview;
  [SerializeField]
  LayerMask spaceshipLayer;
  [SerializeField]
  LineRenderer lineRenderer;

  bool building = false;
  GameObject buildingObject;

  bool buildingConnection = false;
  public bool IsbuildingConnection { get { return buildingConnection; } }
  SpaceShipModule moduleA;

  Camera mainCamera;

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
    preview.gameObject.SetActive(false);
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
    if (collision != null)
    {
      Debug.Log(collision.name);
      preview.color = Color.red;
    }
    else
    {
      preview.color = Color.green;
    }

    if (Input.GetMouseButtonDown(0))
    {
      if (collision == null)
      {
        Instantiate(buildingObject, preview.transform.position, Quaternion.identity);

        SpaceShipModule module = buildingObject.GetComponent<SpaceShipModule>();
        if (module != null)
        {
          ResourceManager.instance.SpendResources(module.BaseCost);
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

  public void StartBuild(GameObject toBuild)
  {
    SpaceShipModule module = toBuild.GetComponent<SpaceShipModule>();
    if (module != null)
    {
      if (!ResourceManager.instance.CanAfford(module.BaseCost))
      {
        return;
      }
    }

    preview.gameObject.SetActive(true);
    building = true;
    buildingObject = toBuild;
  }

  public void StartBuildingConnection()
  {
    if (ResourceManager.instance.CanAfford(new List<ResourceCost>() { new ResourceCost(Resource.Wood, 5) }))
    {
      buildingConnection = true;
      moduleA = null;
    }
  }

  public void ModuleClicked(SpaceShipModule module)
  {
    if (moduleA == null)
    {
      moduleA = module;
      lineRenderer.enabled = true;
      lineRenderer.SetPositions(new Vector3[] {
              moduleA.transform.position,
              mainCamera.ScreenToWorldPoint(Input.mousePosition)
          });
    }
    else
    {
      moduleA.CreateModuleConnection(module);
      buildingConnection = false;
      lineRenderer.enabled = false;

      ResourceManager.instance.SpendResources(new List<ResourceCost>() {
        new ResourceCost(Resource.Wood, 5),
      });
    }
  }
}
