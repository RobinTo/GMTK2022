using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShieldModule : MonoBehaviour, IUpgradable
{
  [SerializeField]
  Animator animator;

  public float cooldown = 10f;
  float timer = 0;

  public int chanceToShieldOnEnter = 25;

  int level = 0;

  [SerializeField]
  List<UpgradeCosts> baseUpgradeCosts;
  [SerializeField]
  CircleCollider2D shieldCollider;

  [Header("Sounds")]
  [SerializeField]
  AudioClip shieldSound;
  AudioSource audioSource;

  // Start is called before the first frame update
  void Start()
  {
    audioSource = gameObject.AddComponent<AudioSource>();
    audioSource.clip = shieldSound;
    audioSource.playOnAwake = false;
    audioSource.volume = SettingsManager.instance.sfxVolume;
    AudioManager.instance.OnSFXVolumeChanged += OnSFXVolumeChanged;
  }

  void OnDestroy()
  {
    AudioManager.instance.OnSFXVolumeChanged -= OnSFXVolumeChanged;
  }

  void OnSFXVolumeChanged(float volume)
  {
    audioSource.volume = volume;
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
          audioSource.Play();
          animator.SetTrigger("Shield");
          timer = cooldown;
          projectile.Shield();
        }

      }
    }
  }
  public void Upgrade()
  {
    level++;
    shieldCollider.radius += shieldCollider.radius / 5;
    chanceToShieldOnEnter += 10;
    cooldown -= cooldown / 10;
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
