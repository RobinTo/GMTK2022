using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairModule : MonoBehaviour, IUpgradable
{
  SpaceShipModule spaceshipModule;


  [SerializeField]
  float repairInterval = 15f;
  float timer = 0;

  [SerializeField]
  float repairChance = 10;
  [SerializeField]
  int repairAmount = 1;

  int level = 0;

  [SerializeField]
  List<UpgradeCosts> baseUpgradeCosts;

  [Header("Sounds")]
  [SerializeField]
  AudioClip healSound;
  AudioSource audioSource;

  // Start is called before the first frame update
  void Start()
  {
    audioSource = gameObject.AddComponent<AudioSource>();
    audioSource.clip = healSound;
    audioSource.playOnAwake = false;
    audioSource.volume = SettingsManager.instance.sfxVolume;
    spaceshipModule = GetComponent<SpaceShipModule>();

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
    timer += Time.deltaTime;
    if (timer < repairInterval) return;
    // Repair one health on a connected module that has lost health if random is less than repair chance
    if (spaceshipModule.health < spaceshipModule.maxHealth && Random.Range(0, 100) < repairChance)
    {
      spaceshipModule.health += repairAmount;
      timer = 0;
    }
    foreach (SpaceShipModule module in spaceshipModule.ConnectedModules)
    {
      if (module.health < module.maxHealth && Random.Range(0, 100) < repairChance)
      {
        audioSource.Play();
        Debug.Log("TODO: Healing animation");
        module.health += repairAmount;
        timer = 0;
        break;
      }
    }

  }

  public void Upgrade()
  {
    level++;
    repairChance += 5;
    if (level % 3 == 0) repairAmount++;
    repairInterval -= repairInterval / 10;
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
