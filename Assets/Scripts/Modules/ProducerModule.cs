using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProducerModule : MonoBehaviour, IUpgradable
{
  [SerializeField]
  GameObject progressbarPrefab;
  [SerializeField]
  float progressbarOffset = 1;
  ProgressBar currentProgressBar;

  [SerializeField]
  float timeToProduce = 15;
  [SerializeField]
  int chanceToFail = 5;

  float timer = 0;

  [SerializeField]
  Resource resourceToProduce;
  [SerializeField]
  int minAmount = 1;
  [SerializeField]
  int maxAmount = 1;

  [SerializeField]
  List<ResourceCost> requiresResources;

  int level = 0;

  [SerializeField]
  List<UpgradeCosts> baseUpgradeCosts;


  [Header("Sounds")]
  [SerializeField]
  AudioClip produceSound;
  [SerializeField]
  AudioClip failSound;
  AudioSource audioSource;

  bool producing = true;
  public bool IsProducing { get { return producing; } }
  public void SetProducing(bool producing)
  {
    this.producing = producing;
  }

  // Start is called before the first frame update
  void Start()
  {
    audioSource = gameObject.AddComponent<AudioSource>();
    audioSource.clip = produceSound;
    audioSource.playOnAwake = false;
    audioSource.volume = SettingsManager.instance.sfxVolume;
    Transform parent = GameObject.FindGameObjectWithTag("UIHealthbarParent").transform;
    currentProgressBar = ObjectPooler.instance.GetPooledObject(progressbarPrefab, Vector3.zero, Quaternion.identity, parent).GetComponent<ProgressBar>();
    currentProgressBar.Attach(gameObject, progressbarOffset);

    AudioManager.instance.OnSFXVolumeChanged += OnSFXVolumeChanged;
  }

  void OnDestroy()
  {
    AudioManager.instance.OnSFXVolumeChanged -= OnSFXVolumeChanged;
    ObjectPooler.instance.Release(progressbarPrefab, currentProgressBar.gameObject);
  }

  void OnSFXVolumeChanged(float volume)
  {
    audioSource.volume = volume;
  }

  // Update is called once per frame
  void Update()
  {
    if (!producing) return;

    timer += Time.deltaTime;
    currentProgressBar.SetProgress(timer / timeToProduce);
    // If timer larger than time to produce, produce a resource with chance to fail
    if (timer >= timeToProduce)
    {
      if (ResourceManager.instance.CanAfford(requiresResources))
      {
        ResourceManager.instance.SpendResources(requiresResources);
        if (Random.Range(0, 100) > chanceToFail)
        {
          audioSource.clip = produceSound;
          audioSource.Play();
          ResourceManager.instance.ModifyResource(resourceToProduce, Random.Range(minAmount, maxAmount + 1));
        }
        else
        {
          audioSource.clip = failSound;
          audioSource.Play();
        }
        timer = 0;
      }
    }
  }

  public void Upgrade()
  {
    level++;
    maxAmount++;
    timeToProduce -= timeToProduce / 10;
    chanceToFail--;
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
