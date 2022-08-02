using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ResourceCost
{
  public Resource resource;
  public int amount;

  public ResourceCost(Resource resource, int amount)
  {
    this.resource = resource;
    this.amount = amount;
  }
}

[CreateAssetMenu(fileName = "ModuleSO", menuName = "Data/ModuleSO", order = 1)]
public class ModuleSO : ScriptableObject
{
  public BuildingId buildingId;
  public Sprite sprite;
  public string moduleName;
  public string moduleDescription;
  public List<ResourceCost> cost;
  public List<UpgradeCosts> baseUpgradeCosts;
  public GameObject prefab;
}
