using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpaceShipModule : MonoBehaviour, IHealth, IPointerClickHandler
{

  [SerializeField]
  GameObject healthbar;
  [SerializeField]
  float healthbarOffset = 3f;

  Healthbar currentHealthbar;

  public int maxHealth = 10;
  public int health = 10;

  Camera mainCamera;
  Transform uiCanvas;

  [Header("Module data")]
  [SerializeField]
  string title;
  [SerializeField]
  string description;
  [SerializeField]
  Sprite icon;

  [SerializeField]
  ModuleConnection moduleConnectionPrefab;

  [SerializeField]
  public List<ResourceCost> BaseCost;

  List<SpaceShipModule> connectedModules;
  public List<SpaceShipModule> ConnectedModules { get { return connectedModules; } }

  public Action<SpaceShipModule> OnModuleDestroyed;

  public void Damage(int damage)
  {
    health -= damage;
    if (health <= 0)
    {
      Debug.Log("Death of " + gameObject.name);
      SpaceShipController.instance.RemoveModule(this);
      OnModuleDestroyed?.Invoke(this);
      Destroy(gameObject);
    }
  }

  void OnDisable()
  {
    if (currentHealthbar)
      ObjectPooler.instance.Release(healthbar, currentHealthbar.gameObject);

    foreach (SpaceShipModule m in connectedModules)
    {
      m.OnModuleDestroyed -= RemoveModuleConnection;
    }
  }

  void Start()
  {
    mainCamera = Camera.main;
    uiCanvas = GameObject.FindGameObjectWithTag("UICanvas").transform;

    // Could potentially be instantiated when creating initial connections when having modules in scene.
    if (connectedModules == null)
      connectedModules = new List<SpaceShipModule>();

    SpaceShipController.instance.AddModule(this);
    if (SpaceShipBrain.instance)
    {
      SpaceShipModule closest = GetClosestSpaceShipModule();
      if (closest != null && closest != this)
        CreateModuleConnection(closest);
    }
  }

  SpaceShipModule GetClosestSpaceShipModule()
  {
    SpaceShipModule closest = SpaceShipBrain.instance.GetComponent<SpaceShipModule>();
    float closestDistance = Vector3.Distance(transform.position, closest.transform.position);
    foreach (var module in SpaceShipController.instance.Modules)
    {
      if (module == this) continue;
      float distance = Vector3.Distance(transform.position, module.transform.position);
      if (distance < closestDistance)
      {
        closest = module;
        closestDistance = distance;
      }
    }
    return closest;
  }

  void Update()
  {

    if (maxHealth > health)
    {
      if (currentHealthbar == null)
      {
        Vector3 position = mainCamera.WorldToScreenPoint(transform.position + Vector3.down * healthbarOffset);
        currentHealthbar = ObjectPooler.instance.GetPooledObject(healthbar, position, Quaternion.identity, uiCanvas).GetComponent<Healthbar>();
      }
      currentHealthbar.SetHealth(health, maxHealth);
    }
    else if (currentHealthbar)
    {
      ObjectPooler.instance.Release(healthbar, currentHealthbar.gameObject);
    }
  }

  public void CreateModuleConnection(SpaceShipModule moduleB)
  {
    ModuleConnection connection = Instantiate(moduleConnectionPrefab, transform);
    connection.Connect(this, moduleB);
    this.AddModuleConnection(moduleB);
    moduleB.AddModuleConnection(this);
  }

  public void AddModuleConnection(SpaceShipModule module)
  {
    if (connectedModules == null)
      connectedModules = new List<SpaceShipModule>();
    connectedModules.Add(module);

    module.OnModuleDestroyed += RemoveModuleConnection;
  }

  void RemoveModuleConnection(SpaceShipModule module)
  {
    connectedModules.Remove(module);
    module.OnModuleDestroyed -= RemoveModuleConnection;
    if (connectedModules.Count == 0)
    {
      OnModuleDestroyed?.Invoke(this);
      SpaceShipController.instance.RemoveModule(this);
      Destroy(gameObject);
    }
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    if (eventData.button == PointerEventData.InputButton.Left)
    {
      if (BuildingManager.instance.IsbuildingConnection)
      {
        BuildingManager.instance.ModuleClicked(this);
        return;
      }

      List<ResourceCost> costs = new List<ResourceCost>();
      IUpgradable upgradeable = GetComponentInChildren<IUpgradable>();
      if (upgradeable != null)
      {
        costs = upgradeable.GetCost();
      }
      ModuleDetailsPanel.instance.Open(title, icon, description, costs, upgradeable);
    }
  }
}
