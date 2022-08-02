using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyModuleButton : MonoBehaviour
{
  public TMP_Text moduleName;
  public TMP_Text moduleDescription;
  public Image sprite;
  public Button button;
  public Transform costContainer;

  ModuleSO scriptableObject;

  public void SetData(ModuleSO data)
  {
    scriptableObject = data;
    moduleName.text = data.moduleName;
    sprite.sprite = data.sprite;
    moduleDescription.text = data.moduleDescription;
  }

}
