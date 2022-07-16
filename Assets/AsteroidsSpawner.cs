using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AsteroidsSpawner : MonoBehaviour
{

  [SerializeField]
  List<Asteroid> normalAsteroids;
  [SerializeField]
  List<Asteroid> rareAsteroids;

  [SerializeField]
  List<Asteroid> epicAsteroids;

  [SerializeField]
  List<Asteroid> legendaryAsteroids;

  [SerializeField]
  float spawnInterval = 1f;
  [SerializeField]
  float spawnChance = 50;
  float timer = 0;




  void SpawnRandomAsteroid()
  {
    Debug.Log("Spawning random asteroid");
    // Get random point on circle centered on transform
    // Get random loot based on rarity
    int randomLoot = Random.Range(0, 100);
    if (randomLoot < 70)
    {
      Asteroid newAsteroid = GetAsteroid(normalAsteroids);
      newAsteroid.Resources.Add(new ResourceCost(Resource.Wood, Random.Range(1, 3)));
      newAsteroid.minSpeed = -.25f;
      newAsteroid.maxSpeed = .25f;
    }
    else if (randomLoot < 90)
    {
      Asteroid newAsteroid = GetAsteroid(rareAsteroids);
      newAsteroid.Resources.Add(new ResourceCost(Resource.Iron, Random.Range(1, 3)));
      newAsteroid.minSpeed = -.5f;
      newAsteroid.maxSpeed = .5f;
    }
    else if (randomLoot < 99)
    {
      Asteroid newAsteroid = GetAsteroid(epicAsteroids);
      newAsteroid.Resources.Add(new ResourceCost(Resource.Gold, Random.Range(1, 3)));
      newAsteroid.minSpeed = -1f;
      newAsteroid.maxSpeed = 1f;
    }
    else
    {
      Asteroid newAsteroid = GetAsteroid(legendaryAsteroids);
      newAsteroid.Resources.Add(new ResourceCost(Resource.Diamond, Random.Range(1, 3)));
      newAsteroid.minSpeed = -2f;
      newAsteroid.maxSpeed = 2f;
    }
  }

  Asteroid GetAsteroid(List<Asteroid> available)
  {

    float angle = Random.Range(0, Mathf.PI * 2);
    float radius = Random.Range(0, GetFurthestModuleFromBrain() * 2);
    Vector3 position = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);

    int randomIndex = Random.Range(0, available.Count);
    Asteroid asteroidPrefab = available[randomIndex];
    Asteroid newAsteroid = ObjectPooler.instance.GetPooledObject(asteroidPrefab.gameObject, SpaceShipBrain.instance.transform.position + position, Quaternion.identity).GetComponent<Asteroid>();
    newAsteroid.enabled = true;
    newAsteroid.GetComponent<TimedPoolObject>().ReturnAfter(asteroidPrefab.gameObject, 30f);
    newAsteroid.Resources = new List<ResourceCost>();
    return newAsteroid;
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
