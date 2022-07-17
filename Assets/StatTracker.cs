using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTracker : MonoBehaviour
{
  public static StatTracker instance;
  public float timeAlive = 0;
  public int enemiesKilled = 0;
  public int circuitBoardsProduced = 0;
  public int advancedCircuitBoardsProduced = 0;

  void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }
    else
    {
      Destroy(gameObject);
    }
  }

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    timeAlive += Time.deltaTime;
  }
}
