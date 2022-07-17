using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
  [SerializeField]
  LineRenderer lineRenderer;

  float closestDistance = Mathf.Infinity;

  // Update is called once per frame
  void Update()
  {
    lineRenderer.SetPositions(new Vector3[] {
            transform.position,
            GetClosestSpaceShipModule().transform.position
        });
  }

  public float GetDistanceToClosestSpaceShipModule()
  {
    return this.closestDistance;
  }

  SpaceShipModule GetClosestSpaceShipModule()
  {
    SpaceShipModule closest = SpaceShipBrain.instance.GetComponent<SpaceShipModule>();
    float closestDistance = Vector3.Distance(transform.position, closest.transform.position);
    foreach (var module in SpaceShipController.instance.Modules)
    {
      if (module == this || module.HasReachedMaxConnections()) continue;
      float distance = Vector3.Distance(transform.position, module.transform.position);
      if (distance < closestDistance)
      {
        closest = module;
        closestDistance = distance;
      }
    }
    this.closestDistance = closestDistance;
    return closest;
  }
}
