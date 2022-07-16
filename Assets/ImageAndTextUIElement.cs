using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImageAndTextUIElement : MonoBehaviour
{
  [SerializeField]
  Image image;
  [SerializeField]
  TMP_Text text;

  public void SetImage(Sprite sprite)
  {
    image.sprite = sprite;
  }
  public void SetText(string text)
  {
    this.text.text = text;
  }
}
