using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldModule : MonoBehaviour
{

  public float cooldown = 10f;
  float timer = 0;

  public int chanceToShieldOnEnter = 25;

  // Start is called before the first frame update
  void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {
    timer -= Time.deltaTime;
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (timer <= 0 && other && other.gameObject && other.gameObject.CompareTag("EnemyProjectile"))
    {
      if (Random.Range(0, 100) < chanceToShieldOnEnter)
      {
        Projectile projectile = other.gameObject.GetComponent<Projectile>();
        if (projectile)
        {
          Debug.Log("Shielded a projectile");
          timer = cooldown;
          projectile.Shield();
        }

      }
    }
  }
}
