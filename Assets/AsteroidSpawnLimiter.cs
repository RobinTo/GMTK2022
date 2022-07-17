using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawnLimiter : MonoBehaviour
{
  [SerializeField]
  float ySpawnOffset = 8;
  [SerializeField]
  float xSpawnOffset = 8;

  float yMinSpawn = -8;
  float yMaxSpawn = 8;
  float xSpawnValue = 13;
  // Used to calculate approximate ttl of asteroid
  float xMinValue = 0;

  // Start is called before the first frame update
  void Start()
  {
    BuildingManager.instance.OnBuildingBuilt += OnBuildingBuilt;
  }

  private void OnBuildingBuilt(GameObject building)
  {
    if (building.transform.position.y - ySpawnOffset < yMinSpawn)
    {
      yMinSpawn = building.transform.position.y - ySpawnOffset;
    }
    if (building.transform.position.y + ySpawnOffset > yMaxSpawn)
    {
      yMaxSpawn = building.transform.position.y + ySpawnOffset;
    }
    if (building.transform.position.x + xSpawnOffset > xSpawnValue)
    {
      xSpawnValue = building.transform.position.x + xSpawnOffset;
    }
  }

  public Vector3 GetAsteroidPosition()
  {
    float yValue = Random.Range(yMinSpawn, yMaxSpawn);
    return new Vector3(xSpawnValue, yValue, 0);
  }

  public float CalculateAsteroidTTL()
  {
    return Mathf.Max(30, Mathf.Abs(xSpawnValue - xMinValue) * 5);
  }

}
