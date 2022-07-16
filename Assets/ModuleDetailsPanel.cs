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

  IUpgradable currentUpgradable;

  void Awake()
  {
    if (instance == null)
      instance = this;
    else
      Destroy(gameObject);
  }

  public void Open(string title, Sprite icon, string description, List<ResourceCost> upgradeCosts, IUpgradable upgradable)
  {
    currentUpgradable = upgradable;
    this.title.text = title;
    this.icon.sprite = icon;
    this.description.text = description;
    for (int i = upgradeCostsContainer.childCount; i > 0; i--)
    {
      Destroy(upgradeCostsContainer.GetChild(i - 1).gameObject);
    }
    foreach (var upgradeCost in upgradeCosts)
    {
      var upgradeCostsUIElement = Instantiate(upgradeCostsUIElementPrefab, upgradeCostsContainer);
      upgradeCostsUIElement.SetImage(ResourceManager.GetSprite(upgradeCost.resource));
      upgradeCostsUIElement.SetText(upgradeCost.amount.ToString());
    }
    gameObject.SetActive(true);
  }

  public void OnUpgradeClick()
  {
    if (currentUpgradable != null && CanAffordToUpgrade()) {
      ResourceManager.instance.SpendResources(currentUpgradable.GetCost());
      currentUpgradable.Upgrade();
    
    }
  }

  bool CanAffordToUpgrade() {
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
