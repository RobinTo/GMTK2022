using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModuleDetailsPanel : MonoBehaviour
{
  public static ModuleDetailsPanel instance;

  [SerializeField]
  TMP_Text title;
  [SerializeField]
  Image icon;
  [SerializeField]
  TMP_Text description;
  [SerializeField]
  Transform upgradeCostsContainer;
  [SerializeField]
  ImageAndTextUIElement upgradeCostsUIElementPrefab;

  [SerializeField]
  Button disableProductionButton;
  [SerializeField]
  TMP_Text disableProductionButtonText;

  IUpgradable currentUpgradable;
  SpaceShipModule currentModule;

  void Awake()
  {
    if (instance == null)
      instance = this;
    else
      Destroy(gameObject);
  }

  public void Open(string title, Sprite icon, string description, List<ResourceCost> upgradeCosts, IUpgradable upgradable, SpaceShipModule module)
  {
    currentModule = module;
    currentUpgradable = upgradable;
    this.title.text = title;
    this.icon.sprite = icon;
    this.description.text = description;
    UpdateUpgradeCost();
    gameObject.SetActive(true);

    ProducerModule pm = module.GetComponent<ProducerModule>();
    disableProductionButton.gameObject.SetActive(pm != null);
    if (pm != null)
    {
      disableProductionButtonText.text = pm.IsProducing ? "Pause Production" : "Start Production";
    }
  }

  public void OnToggleProductionClick()
  {
    ProducerModule pm = currentModule.GetComponent<ProducerModule>();
    if (pm != null)
    {
      pm.SetProducing(!pm.IsProducing);
      disableProductionButtonText.text = pm.IsProducing ? "Pause Production" : "Start Production";
    }
  }

  void UpdateUpgradeCost()
  {
    for (int i = upgradeCostsContainer.childCount; i > 0; i--)
    {
      Destroy(upgradeCostsContainer.GetChild(i - 1).gameObject);
    }
    foreach (var upgradeCost in currentUpgradable.GetCost())
    {
      var upgradeCostsUIElement = Instantiate(upgradeCostsUIElementPrefab, upgradeCostsContainer);
      upgradeCostsUIElement.SetImage(ResourceManager.GetSprite(upgradeCost.resource));
      upgradeCostsUIElement.SetText(upgradeCost.amount.ToString());
    }
  }

  public void OnUpgradeClick()
  {
    if (currentUpgradable != null && CanAffordToUpgrade())
    {
      ResourceManager.instance.SpendResources(currentUpgradable.GetCost());
      currentUpgradable.Upgrade();
      UpdateUpgradeCost();
    }
  }

  bool CanAffordToUpgrade()
  {
    foreach (var upgradeCost in currentUpgradable.GetCost())
    {
      if (ResourceManager.instance.GetResourceAmount(upgradeCost.resource) < upgradeCost.amount)
        return false;
    }
    return true;
  }

  void Start()
  {
    gameObject.SetActive(false);
  }

  void Close()
  {
    gameObject.SetActive(false);
  }
}
