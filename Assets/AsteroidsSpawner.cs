using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AsteroidsSpawner : MonoBehaviour
{
  public static AsteroidsSpawner instance;

  [SerializeField]
  AsteroidSpawnLimiter limiter;

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
  float increaseChanceTimer = 0;
  [SerializeField]
  float increaseChanceInterval = 60;

  [Header("Spawn Chances")]
  [SerializeField]
  int normalAsteroidChance = 80;
  [SerializeField]
  int rareAsteroidChance = 20;
  [SerializeField]
  int epicAsteroidChance = 10;
  [SerializeField]
  int legendaryAsteroidChance = 1;


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


  void SpawnRandomAsteroid()
  {
    // Get random point on circle centered on transform
    // Get random loot based on rarity
    int randomLoot = Random.Range(0, 100);
    if (randomLoot < normalAsteroidChance)
    {
      Asteroid newAsteroid = GetAsteroid(normalAsteroids);
      newAsteroid.Resources.Add(new ResourceCost(Resource.Wood, Random.Range(5, 10)));
      newAsteroid.minSpeed = -.25f;
      newAsteroid.maxSpeed = .25f;
    }
    randomLoot = Random.Range(0, 100);
    if (randomLoot < rareAsteroidChance)
    {
      Asteroid newAsteroid = GetAsteroid(rareAsteroids);
      newAsteroid.Resources.Add(new ResourceCost(Resource.Iron, Random.Range(4, 8)));
      newAsteroid.minSpeed = -.5f;
      newAsteroid.maxSpeed = .5f;
    }
    randomLoot = Random.Range(0, 100);
    if (randomLoot < epicAsteroidChance)
    {
      Asteroid newAsteroid = GetAsteroid(epicAsteroids);
      newAsteroid.Resources.Add(new ResourceCost(Resource.Gold, Random.Range(1, 5)));
      newAsteroid.minSpeed = -1f;
      newAsteroid.maxSpeed = 1f;
    }
    randomLoot = Random.Range(0, 100);
    if (randomLoot < legendaryAsteroidChance)
    {
      Asteroid newAsteroid = GetAsteroid(legendaryAsteroids);
      newAsteroid.Resources.Add(new ResourceCost(Resource.Diamond, Random.Range(1, 2)));
      newAsteroid.minSpeed = -2f;
      newAsteroid.maxSpeed = 2f;
    }
  }

  public void IncreaseAsteroidChance()
  {
    normalAsteroidChance += 1;
    rareAsteroidChance += 1;
    epicAsteroidChance += 1;
    legendaryAsteroidChance += 1;
  }

  Asteroid GetAsteroid(List<Asteroid> available)
  {


    Vector3 position = limiter.GetAsteroidPosition();

    int randomIndex = Random.Range(0, available.Count);
    Asteroid asteroidPrefab = available[randomIndex];
    Asteroid newAsteroid = ObjectPooler.instance.GetPooledObject(asteroidPrefab.gameObject, SpaceShipBrain.instance.transform.position + position, Quaternion.identity).GetComponent<Asteroid>();
    newAsteroid.enabled = true;
    newAsteroid.GetComponent<TimedPoolObject>().ReturnAfter(asteroidPrefab.gameObject, limiter.CalculateAsteroidTTL());
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
    return furthest > 8 ? furthest : 8;
  }

  // Start is called before the first frame update
  void Start()
  {

  }



  // Update is called once per frame
  void Update()
  {
    if (!GameManager.instance.GameOn) return;

    // Spawn asteroid when timer reached spawnInterval if random number is less than spawnChance
    timer += Time.deltaTime;
    increaseChanceTimer += Time.deltaTime;
    if (timer >= spawnInterval)
    {
      if (Random.Range(0, 100) < spawnChance)
        SpawnRandomAsteroid();
      timer = 0;
    }
    if (increaseChanceTimer > increaseChanceInterval)
    {
      IncreaseAsteroidChance();
      increaseChanceTimer = 0;
    }
  }

  public void IncreaseChancesAfterBoss()
  {
    normalAsteroidChance = 50;
    rareAsteroidChance = 50;
    epicAsteroidChance += 15;
    legendaryAsteroidChance += 3;
  }
}
