using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHealth
{
  public int health = 5;

  public void Damage(int health)
  {
    this.health -= health;
    if (this.health <= 0)
    {
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
