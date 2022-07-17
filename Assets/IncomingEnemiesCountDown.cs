using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IncomingEnemiesCountDown : MonoBehaviour
{
  TMP_Text text;
  // Start is called before the first frame update
  void Start()
  {
    text = GetComponent<TMP_Text>();
  }

  // Update is called once per frame
  void Update()
  {
    float remainingTime = EnemySpawner.instance.RemainingTime();
    text.text = "Incoming in " + String.Format("{0:0.00}", remainingTime);
    if (remainingTime <= 0)
    {
      gameObject.SetActive(false);
    }
  }
}
