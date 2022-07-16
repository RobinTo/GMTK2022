using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AsteroidsSpawner : MonoBehaviour
{

  [SerializeField]
  List<Asteroid> asteroids;

  [SerializeField]
  float spawnInterval = 1f;
  [SerializeField]
  float spawnChance = 50;
  float timer = 0;




  void SpawnRandomAsteroid()
  {
    Debug.Log("Spawning random asteroid");
    // Get random point on circle centered on transform
    float angle = Random.Range(0, Mathf.PI * 2);
    float radius = Random.Range(0, GetFurthestModuleFromBrain() * 2);
    Vector3 position = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);

    int randomIndex = Random.Range(0, asteroids.Count);
    Asteroid asteroidPrefab = asteroids[randomIndex];
    Asteroid newAsteroid = Instantiate(asteroidPrefab, SpaceShipBrain.instance.transform.position + position, Quaternion.identity);

    // Get random loot based on rarity
    int randomLoot = Random.Range(0, 100);
    newAsteroid.Resources = new List<ResourceCost>();
    if (randomLoot < 70)
    {
      newAsteroid.Resources.Add(new ResourceCost(Resource.Wood, Random.Range(1, 3)));
      newAsteroid.minSpeed = -.25f;
      newAsteroid.maxSpeed = .25f;
    }
    else if (randomLoot < 90)
    {
      newAsteroid.Resources.Add(new ResourceCost(Resource.Iron, Random.Range(1, 3)));
      newAsteroid.minSpeed = -.5f;
      newAsteroid.maxSpeed = .5f;
    }
    else if (randomLoot < 99)
    {
      newAsteroid.Resources.Add(new ResourceCost(Resource.Gold, Random.Range(1, 3)));
      newAsteroid.minSpeed = -1f;
      newAsteroid.maxSpeed = 1f;
    }
    else
    {
      newAsteroid.Resources.Add(new ResourceCost(Resource.Diamond, Random.Range(1, 3)));
      newAsteroid.minSpeed = -2f;
      newAsteroid.maxSpeed = 2f;
    }
  }

  float GetFurthestModuleFromBrain()
  {
    float furthest = 0;
    foreach (SpaceShipModule module in SpaceShipController.instance.Modules)
    {
      float distance = Vector3.Distance(SpaceShipBrain.instance.transform.position, module.transform.position);
      if (distance > furthest)
      {
        furthest = distance;
      }
    }
    return furthest;
  }

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    // Spawn asteroid when timer reached spawnInterval if random number is less than spawnChance
    timer += Time.deltaTime;
    if (timer >= spawnInterval)
    {
      Debug.Log("trying to spawn random asteroid");
      if (Random.Range(0, 100) < spawnChance)
        SpawnRandomAsteroid();
      timer = 0;
    }
  }
}
