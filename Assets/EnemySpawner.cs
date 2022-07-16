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

  float timer = 0;

  bool active = false;

  [SerializeField]
  float spawnInterval = 15;
  [SerializeField]
  int numberOfEnemies = 1;
  float spawnTimer = 0;

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

  // Update is called once per frame
  void Update()
  {
    if (!active) return;
    timer += Time.deltaTime;

    if (timer > spawnInterval)
    {
      timer = 0;
      for (int i = 0; i < numberOfEnemies; i++)
      {
        SpawnEnemy();
      }
    }
  }

  void SpawnEnemy()
  {
    List<GameObject> availableEnemies = new List<GameObject>();
    foreach (EnemySpawnConditions condition in spawnConditions)
    {
      if (condition.time <= timer)
      {
        availableEnemies.Add(condition.prefab);
      }
    }
    // Spawn random enemy from available enemies
    if (availableEnemies.Count > 0)
    {
      int index = Random.Range(0, availableEnemies.Count);
      Instantiate(availableEnemies[index], tempSpawnPosition.position, Quaternion.identity);
    }
  }
}
