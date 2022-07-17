using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileFire : MonoBehaviour
{
  [SerializeField]
  Projectile projectilePrefab;
  [SerializeField]
  GameObject explosionPrefab;
  [SerializeField]
  GameObject explosionShieldedPrefab;
  [SerializeField]
  int damage = 1;
  [SerializeField]
  float timeBetweenFire = 0.5f;
  float timer = 0;
  [SerializeField]
  List<Transform> spawnPositions;

  [SerializeField]
  AudioSource audioSource;


  // Start is called before the first frame update
  void Start()
  {
    audioSource.volume = SettingsManager.instance.sfxVolume;
    AudioManager.instance.OnSFXVolumeChanged += OnSFXVolumeChanged;
  }

  void OnSFXVolumeChanged(float volume)
  {
    audioSource.volume = volume;
  }

  // Update is called once per frame
  void Update()
  {
    // Fire projectile when timer reached timeBetweenFire
    timer += Time.deltaTime;
    if (timer >= timeBetweenFire)
    {
      // Spawn projectile at random spawn position
      int randomIndex = Random.Range(0, spawnPositions.Count);
      Projectile projectile = ObjectPooler.instance.GetPooledObject(projectilePrefab.gameObject, spawnPositions[randomIndex].position, spawnPositions[randomIndex].rotation).GetComponent<Projectile>();
      audioSource.Play();
      projectile.ResetTTL();
      projectile.damage = damage;
      projectile.onHit = OnProjectileHit;
      projectile.onFadeAway = OnProjectileHit;
      projectile.onGotShielded = OnProjectileShielded;
      timer = 0;
    }
  }

  void OnProjectileHit(Projectile projectile)
  {
    ObjectPooler.instance.Release(projectilePrefab.gameObject, projectile.gameObject);

    ITimedPoolObject explosion = ObjectPooler.instance.GetPooledObject(explosionPrefab, projectile.transform.position, projectile.transform.rotation).GetComponent<ITimedPoolObject>();
    explosion.ReturnAfter(explosionPrefab, 1f);
  }

  void OnProjectileShielded(Projectile projectile)
  {
    ObjectPooler.instance.Release(projectilePrefab.gameObject, projectile.gameObject);

    ITimedPoolObject explosion = ObjectPooler.instance.GetPooledObject(explosionShieldedPrefab, projectile.transform.position, projectile.transform.rotation).GetComponent<ITimedPoolObject>();
    explosion.ReturnAfter(explosionPrefab, 1f);
  }
}
