using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleConnection : MonoBehaviour
{
  SpaceShipModule moduleA;
  SpaceShipModule moduleB;


  public void Connect(SpaceShipModule moduleA, SpaceShipModule moduleB)
  {
    this.moduleA = moduleA;
    this.moduleB = moduleB;

    LineRenderer lineRenderer = GetComponent<LineRenderer>();
    lineRenderer.SetPositions(new Vector3[] {
            moduleA.transform.position,
            moduleB.transform.position
        });
  }
}
