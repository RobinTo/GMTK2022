using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerModule : MonoBehaviour
{
  public int chanceToMine = 5;
  public float timeBetweenMines = 1f;
  public float timer = 0;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    timer += Time.deltaTime;
    // Try to mine when timer reached timebetween mine with a chance of chanceToMine
    if (timer >= timeBetweenMines)
    {
      if (Random.Range(0, chanceToMine) == 0)
      {
        // Add a metal to spaceshipcontroller
        SpaceShipController.instance.AddMetal(1);
      }
      timer = 0;
    }
  }
}
