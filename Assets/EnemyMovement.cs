using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

  SpaceShipModule target;
  Vector3 velocity = Vector3.zero;

  // Start is called before the first frame update
  void Start()
  {
    target = FindClosestEnemy(SpaceShipController.instance.Modules);
  }

  // Update is called once per frame
  void Update()
  {
    if (target == null)
    {
      target = FindClosestEnemy(SpaceShipController.instance.Modules);
      return;
    }

    transform.rotation = Quaternion.Slerp(transform.rotation, GetLookRotation(), Time.deltaTime * 10f);

    if (Vector3.Distance(transform.position, target.transform.position) > 3f)
    {
      velocity += transform.right * Time.deltaTime;
      velocity = Vector3.ClampMagnitude(velocity, 4f);
    }
    transform.position += velocity * Time.deltaTime;
  }

  private Quaternion GetLookRotation()
  {
    Vector3 dir = target.transform.position - transform.position;
    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    return Quaternion.AngleAxis(angle, Vector3.forward);
  }

  // Function to find the closest target in the list of modules in spaceshipcontroller
  public SpaceShipModule FindClosestEnemy(List<SpaceShipModule> modules)
  {
    SpaceShipModule tMin = null;
    float minDist = Mathf.Infinity;
    foreach (SpaceShipModule module in modules)
    {
      float dist = Vector3.Distance(module.transform.position, transform.position);
      if (dist < minDist)
      {
        tMin = module;
        minDist = dist;
      }
    }
    return tMin;
  }
}
