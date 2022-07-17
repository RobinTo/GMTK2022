using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingRocketModule : MonoBehaviour, IUpgradable
{
  [SerializeField]
  HomingProjectile bulletPrefab;
  [SerializeField]
  GameObject explosionPrefab;
  [SerializeField]
  GameObject explosionShieldedPrefab;
  [SerializeField]
  Transform shootPosition;

  public int range = 10;
  public float chanceToFire = 5f;

  public float timeBetweenFires = 3f;
  public float timer = 0f;

  GameObject target;

  int level = 0;
  int baseDamage = 10;
  [SerializeField]
  List<UpgradeCosts> baseUpgradeCosts;

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

    timer += Time.deltaTime;
    // Try to fire when timer reaches timebetween fire with a chance of chanceToFire
    if (timer >= timeBetweenFires)
    {
      if (target != null && Random.Range(0, 100) < chanceToFire)
      {
        HomingProjectile projectile = ObjectPooler.instance.GetPooledObject(bulletPrefab.gameObject, shootPosition.position, shootPosition.rotation).GetComponent<HomingProjectile>();
        projectile.SetTarget(target.transform);
        projectile.damage = baseDamage;
        projectile.onHit = OnProjectileHit;
        projectile.onFadeAway += OnProjectileHit;
        projectile.onGotShielded += OnProjectileShielded;
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

  void OnProjectileHit(HomingProjectile projectile, Transform target)
  {
    ObjectPooler.instance.Release(bulletPrefab.gameObject, projectile.gameObject);

    ITimedPoolObject explosion = ObjectPooler.instance.GetPooledObject(explosionPrefab, projectile.transform.position, projectile.transform.rotation).GetComponent<ITimedPoolObject>();
    explosion.ReturnAfter(explosionPrefab, 1f);
  }

  void OnProjectileShielded(HomingProjectile projectile, Transform target)
  {
    ObjectPooler.instance.Release(bulletPrefab.gameObject, projectile.gameObject);

    ITimedPoolObject explosion = ObjectPooler.instance.GetPooledObject(explosionShieldedPrefab, projectile.transform.position, projectile.transform.rotation).GetComponent<ITimedPoolObject>();
    explosion.ReturnAfter(explosionPrefab, 1f);
  }

  // Function to check if any enemy is in range
  public bool CheckEnemyInRange(Enemy enemy)
  {
    return Vector3.Distance(transform.position, enemy.transform.position) <= range;
  }

  public void Upgrade()
  {
    level++;
    baseDamage += 1;
    chanceToFire += 5;
    timeBetweenFires -= timeBetweenFires / 10f;
  }
  public List<ResourceCost> GetCost()
  {
    if (baseUpgradeCosts.Count > level)
    {
      return baseUpgradeCosts[level].costs;
    }
    else
    {
      List<ResourceCost> cost = new List<ResourceCost>();
      List<ResourceCost> lastResourceCost = baseUpgradeCosts[baseUpgradeCosts.Count - 1].costs;
      for (int i = 0; i < lastResourceCost.Count; i++)
      {
        cost.Add(new ResourceCost(lastResourceCost[i].resource, lastResourceCost[i].amount * level));
      }
      return cost;
    }
  }
}
