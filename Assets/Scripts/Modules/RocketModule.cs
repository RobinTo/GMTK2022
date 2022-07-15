using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketModule : MonoBehaviour
{
  [SerializeField]
  Projectile bulletPrefab;
  [SerializeField]
  GameObject explosionPrefab;
  [SerializeField]
  Transform shootPosition;

  public int range = 10;
  public float chanceToFire = 5f;

  public float timeBetweenFires = 1f;
  public float timer = 0f;

  GameObject target;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (target == null || Vector3.Distance(shootPosition.position, target.transform.position) > range)
    {
      TryFindNewTarget();
    }
    else
    {
      transform.rotation = Quaternion.Slerp(transform.rotation, GetLookRotation(), Time.deltaTime * 10f);
    }
    timer += Time.deltaTime;
    // Try to fire when timer reaches timebetween fire with a chance of chanceToFire
    if (timer >= timeBetweenFires)
    {
      if (Random.Range(0, 100) < chanceToFire)
      {
        Debug.Log("Fire projectile!");
        Projectile projectile = ObjectPooler.instance.GetPooledObject(bulletPrefab.gameObject, shootPosition.position, shootPosition.rotation).GetComponent<Projectile>();
        projectile.onHit = OnProjectileHit;
        projectile.onFadeAway += OnProjectileHit;
        projectile.onGotShielded += OnProjectileHit;
      }
      timer = 0;
    }
  }

  void TryFindNewTarget()
  {
    foreach (Enemy enemy in GameManager.instance.Enemies)
    {
      if (Vector3.Distance(shootPosition.position, enemy.transform.position) < range)
      {
        target = enemy.gameObject;
        break;
      }
    }
  }


  private Quaternion GetLookRotation()
  {
    Vector3 dir = target.transform.position - transform.position;
    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    return Quaternion.AngleAxis(angle, Vector3.forward);
  }

  void OnProjectileHit(Projectile projectile)
  {
    ObjectPooler.instance.Release(bulletPrefab.gameObject, projectile.gameObject);

    ITimedPoolObject explosion = ObjectPooler.instance.GetPooledObject(explosionPrefab, projectile.transform.position, projectile.transform.rotation).GetComponent<ITimedPoolObject>();
    explosion.ReturnAfter(explosionPrefab, 1f);
  }

  // Function to check if any enemy is in range
  public bool CheckEnemyInRange(Enemy enemy)
  {
    return Vector3.Distance(transform.position, enemy.transform.position) <= range;
  }
}
