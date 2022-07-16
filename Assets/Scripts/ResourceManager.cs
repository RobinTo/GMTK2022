using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
  public static ResourceManager instance;

  Dictionary<Resource, int> resources;

  public Action<Resource, int> OnResourceChanged;

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
    resources = new Dictionary<Resource, int>();
  }

  // Start is called before the first frame update
  void Start()
  {

    ModifyResource(Resource.Wood, 30);
    ModifyResource(Resource.Iron, 30);
  }

  public int GetResourceAmount(Resource resource)
  {
    if (!resources.ContainsKey(resource))
      return 0;
    return resources[resource];
  }

  public void ModifyResource(Resource resource, int modifier)
  {
    if (!resources.ContainsKey(resource))
    {
      resources.Add(resource, modifier);
    }
    else
    {
      resources[resource] += modifier;
    }
    OnResourceChanged?.Invoke(resource, resources[resource]);
  }

  public void SpendResources(List<ResourceCost> cost)
  {
    foreach (ResourceCost resourceCost in cost)
    {
      ModifyResource(resourceCost.resource, -resourceCost.amount);
    }
  }

  public bool CanAfford(List<ResourceCost> cost)
  {
    foreach (ResourceCost resourceCost in cost)
    {
      if (GetResourceAmount(resourceCost.resource) < resourceCost.amount)
      {
        return false;
      }
    }
    return true;
  }

  public static Sprite GetSprite(Resource resource)
  {
    switch (resource)
    {
      case Resource.Wood:
        return Resources.Load<Sprite>("wood");
      case Resource.Iron:
        return Resources.Load<Sprite>("iron");
      case Resource.Gold:
        return Resources.Load<Sprite>("gold");
      case Resource.Diamond:
        return Resources.Load<Sprite>("diamond");
    }
    return null;

  }
}
