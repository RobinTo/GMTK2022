using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
  [SerializeField]
  Image fillImage;
  [SerializeField]
  TMP_Text text;

  public void SetHealth(int health, int maxHealth)
  {
    text.text = health + "/" + maxHealth;
    fillImage.fillAmount = (float)health / (float)maxHealth;
  }
}
