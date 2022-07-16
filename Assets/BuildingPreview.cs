using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
  [SerializeField]
  LineRenderer lineRenderer;


  // Update is called once per frame
  void Update()
  {
    lineRenderer.SetPositions(new Vector3[] {
            transform.position,
            GetClosestSpaceShipModule().transform.position
        });
  }
  SpaceShipModule GetClosestSpaceShipModule()
  {
    SpaceShipModule closest = SpaceShipBrain.instance.GetComponent<SpaceShipModule>();
    float closestDistance = Vector3.Distance(transform.position, closest.transform.position);
    foreach (var module in SpaceShipController.instance.Modules)
    {
      if (module == this) continue;
      float distance = Vector3.Distance(transform.position, module.transform.position);
      if (distance < closestDistance)
      {
        closest = module;
        closestDistance = distance;
      }
    }
    return closest;
  }
}
