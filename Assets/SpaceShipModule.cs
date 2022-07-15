using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpaceShipModule : MonoBehaviour, IHealth
{
  [SerializeField]
  LineRenderer lineRenderer;

  public int health = 10;

  public void Damage(int damage)
  {
    health -= damage;
    if (health <= 0)
    {
      SpaceShipController.instance.RemoveModule(this);
      Destroy(gameObject);
    }
  }

  void Start()
  {
    SpaceShipController.instance.AddModule(this);
    if (lineRenderer && SpaceShipBrain.instance)
      lineRenderer.SetPositions(new Vector3[] { transform.position, SpaceShipBrain.instance.transform.position });
  }

  void Update()
  {
    if (lineRenderer && SpaceShipBrain.instance)
    {
      lineRenderer.SetPosition(0, transform.position);
      lineRenderer.SetPosition(1, SpaceShipBrain.instance.transform.position);
    }
  }
}
