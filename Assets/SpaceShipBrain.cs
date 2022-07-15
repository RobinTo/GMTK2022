using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipBrain : MonoBehaviour
{
  public static SpaceShipBrain instance;


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

  }
}
