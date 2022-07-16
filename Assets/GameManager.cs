using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
  }

  // Update is called once per frame
  void Update()
  {

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
