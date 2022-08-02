using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemySpawnConditions
{
  public float time;
  public GameObject prefab;

  public EnemySpawnConditions(float time, GameObject prefab)
  {
    this.time = time;
    this.prefab = prefab;
  }
}

public class EnemySpawner : MonoBehaviour
{
  public static EnemySpawner instance;

  [SerializeField]
  List<EnemySpawnConditions> spawnConditions;
  [SerializeField]
  Transform tempSpawnPosition;

  [SerializeField]
  GameObject bossEnemy;
  bool spawningBoss = false;
  float bossTimer = 0;

  float timer = 0;

  bool active = false;

  [SerializeField]
  float spawnInterval = 15;
  [SerializeField]
  int numberOfEnemies = 1;
  float spawnTimer = 0;

  [SerializeField]
  float initialWaitTime = 30;

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

  // Start is called before the first frame update
  void Start()
  {

  }

  public void SetActive(bool value)
  {
    active = value;
  }

  public void TriggerBossSpawn(float time)
  {
    spawningBoss = true;
    bossTimer = time;
  }

  public bool initialSpawnHappened = false;

  public float RemainingTime()
  {
    return initialSpawnHappened ? 0 : initialWaitTime + spawnInterval - timer;
  }

  // Update is called once per frame
  void Update()
  {
    if (!active || !GameManager.instance.GameOn) return;
    if (initialWaitTime > 0)
    {
      initialWaitTime -= Time.deltaTime;
      return;
    }

    if (spawningBoss)
    {
      bossTimer -= Time.deltaTime;
      if (bossTimer <= 0)
      {
        spawningBoss = false;
        SpawnBossEnemy();
      }
      return;
    }

    timer += Time.deltaTime;

    if (timer > spawnInterval)
    {
      timer = 0;
      initialSpawnHappened = true;
      for (int i = 0; i < Random.Range(1, numberOfEnemies + 1); i++)
      {
        SpawnEnemy();

        if (spawnInterval > 2 && Random.Range(0, 10) > 8)
        {
          spawnInterval -= Random.Range(0f, 1f);
        }
        if (Random.Range(0, 100) > 95)
        {
          numberOfEnemies += 1;
        }
      }
    }
  }
  void SpawnBossEnemy()
  {

    Vector3 point = Random.insideUnitCircle.normalized;
    float radius = GetFurthestModuleFromBrain() * 3;
    Vector3 position = new Vector3(point.x * radius, point.y * radius, 0);

    Instantiate(bossEnemy, SpaceShipBrain.instance.transform.position + position, Quaternion.identity);
  }
  void SpawnEnemy()
  {
    List<GameObject> availableEnemies = new List<GameObject>();
    foreach (EnemySpawnConditions condition in spawnConditions)
    {
      if (condition.time <= StatTracker.instance.timeAlive)
      {
        availableEnemies.Add(condition.prefab);
      }
    }
    // Spawn random enemy from available enemies
    if (availableEnemies.Count > 0)
    {
      int index = Random.Range(0, availableEnemies.Count);

      Vector3 point = Random.insideUnitCircle.normalized;
      float radius = GetFurthestModuleFromBrain() * 3;
      Vector3 position = new Vector3(point.x * radius, point.y * radius, 0);

      Instantiate(availableEnemies[index], SpaceShipBrain.instance.transform.position + position, Quaternion.identity);
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
    return furthest > 8 ? furthest : 8;
  }
}
