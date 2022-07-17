using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  public static GameManager instance;

  List<Enemy> enemies;
  public List<Enemy> Enemies
  {
    get
    {
      return enemies;
    }
  }

  public bool GameOn = true;

  public UnityEvent OnGameOver;

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
    enemies = new List<Enemy>();
  }

  // Start is called before the first frame update
  void Start()
  {
    SpaceShipBrain.instance.GetComponent<SpaceShipModule>().OnModuleDestroyed += EndGame;
  }

  void EndGame(SpaceShipModule module)
  {
    GameOn = false;
    OnGameOver?.Invoke();
  }

  // Update is called once per frame
  void Update()
  {
    if (!GameOn)
    {
      if (Input.GetKeyDown(KeyCode.R))
      {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
      }
    }
  }

  public void AddEnemy(Enemy enemy)
  {
    enemies.Add(enemy);
  }

  public void RemoveEnemy(Enemy enemy)
  {
    enemies.Remove(enemy);
  }
}
