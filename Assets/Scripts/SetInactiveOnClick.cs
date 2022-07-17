using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetInactiveOnClick : MonoBehaviour, IPointerClickHandler
{
  public void OnPointerClick(PointerEventData eventData)
  {
    gameObject.SetActive(false);
  }
}
