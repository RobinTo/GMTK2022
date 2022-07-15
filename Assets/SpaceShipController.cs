using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipController : MonoBehaviour
{
  public static SpaceShipController instance;

  List<SpaceShipModule> modules = new List<SpaceShipModule>();
  public List<SpaceShipModule> Modules { get { return modules; } }

  // Resources
  private int metal;
  public int Metal { get { return metal; } }


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

  public void AddModule(SpaceShipModule module)
  {
    modules.Add(module);
  }

  public void RemoveModule(SpaceShipModule module)
  {
    if (SpaceShipBrain.instance && module.gameObject == SpaceShipBrain.instance.gameObject)
    {
      Debug.Log("Space ship brain was destroyed!");
      // Game ends!
    }
    modules.Remove(module);
  }

  public void AddMetal(int metal)
  {
    this.metal += metal;
  }
}
