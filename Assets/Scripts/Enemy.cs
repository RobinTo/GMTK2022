using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHealth
{
  public static Action<Enemy> OnEnemyDestroyed;
  public int health = 5;
  public bool isBoss = false;

  public void Damage(int health)
  {
    this.health -= health;
    if (this.health <= 0)
    {
      OnEnemyDestroyed?.Invoke(this);
      Destroy(gameObject);
    }
  }

  // Start is called before the first frame update
  void Start()
  {
    GameManager.instance.AddEnemy(this);
  }

  void OnDisable()
  {
    GameManager.instance.RemoveEnemy(this);
  }

  // Update is called once per frame
  void Update()
  {

  }
}
