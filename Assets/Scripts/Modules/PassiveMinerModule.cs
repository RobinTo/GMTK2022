using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ResourceProductionInterval
{
  public Resource resource;
  public int minAmount;
  public int maxAmount;
}

public class PassiveMinerModule : MonoBehaviour, IUpgradable
{
  [SerializeField]
  GameObject progressbarPrefab;
  [SerializeField]
  float progressbarOffset = 1;
  ProgressBar currentProgressBar;

  [SerializeField]
  List<ResourceProductionInterval> resourcesToProduce;
  [SerializeField]
  GameObject scrollingResourcePrefab;

  [SerializeField]
  float timeToProduce = 30;

  float timer = 0;

  [SerializeField]
  int minAmount = 1;
  [SerializeField]
  float resourceProductionMultiplier = 1;

  int level = 0;

  [SerializeField]
  List<UpgradeCosts> baseUpgradeCosts;

  Camera mainCamera;
  Transform parent;

  [Header("Sounds")]
  [SerializeField]
  AudioClip produceSound;
  AudioSource audioSource;

  // Start is called before the first frame update
  void Start()
  {
    audioSource = gameObject.AddComponent<AudioSource>();
    audioSource.clip = produceSound;
    audioSource.playOnAwake = false;
    audioSource.volume = SettingsManager.instance.sfxVolume;
    mainCamera = Camera.main;
    parent = GameObject.FindGameObjectWithTag("UIHealthbarParent").transform;
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
    // Produce a random resource after timeToProduce has passed
    timer += Time.deltaTime;
    currentProgressBar.SetProgress(timer / timeToProduce);
    if (timer >= timeToProduce)
    {
      List<Tuple<Resource, int>> resources = new List<Tuple<Resource, int>>();
      foreach (ResourceProductionInterval interval in resourcesToProduce)
      {
        int amount = UnityEngine.Random.Range(interval.minAmount, interval.maxAmount);
        ResourceManager.instance.ModifyResource(interval.resource, amount);
        resources.Add(new Tuple<Resource, int>(interval.resource, amount));
      }
      StartCoroutine(ShowGains(resources));
      timer = 0;
    }
  }

  IEnumerator ShowGains(List<Tuple<Resource, int>> resources)
  {
    foreach (Tuple<Resource, int> resource in resources)
    {
      audioSource.Play();
      ShowScrollingResourcePrefab(resource.Item1, resource.Item2);
      yield return new WaitForSeconds(0.25f);
    }
  }
  void ShowScrollingResourcePrefab(Resource resource, int amount)
  {
    Vector3 position = mainCamera.WorldToScreenPoint(transform.position + Vector3.up * .5f);
    ImageAndTextUIElement scrollingResource = ObjectPooler.instance.GetPooledObject(scrollingResourcePrefab, position, Quaternion.identity, parent).GetComponent<ImageAndTextUIElement>();
    scrollingResource.SetImage(ResourceManager.GetSprite(resource));
    scrollingResource.SetText((amount >= 0 ? "+" : "-") + amount.ToString());
    scrollingResource.GetComponent<TimedPoolObject>().ReturnAfter(scrollingResourcePrefab, 2f);
  }

  public void Upgrade()
  {
    level++;
    resourceProductionMultiplier += resourceProductionMultiplier * .25f;
    timeToProduce -= timeToProduce / 10;
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
