using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Asteroid : MonoBehaviour
{
  float speed = 1f;
  Vector3 velocity;
  public float minSpeed = -1;
  public float maxSpeed = 1;

  public bool IsGrabbed = false;

  List<ResourceCost> resources;

  public List<ResourceCost> Resources
  {
    get { return resources; }
    set { resources = value; }
  }

  // Start is called before the first frame update
  void OnEnable()
  {
    IsGrabbed = false;
    transform.parent = null;
    // Set a random velocity
    velocity = new Vector3(Random.Range(minSpeed, maxSpeed), Random.Range(minSpeed, maxSpeed), 0);

    // Set a random rotation
    transform.Rotate(0, 0, Random.Range(0f, 360f));
  }


  // Update is called once per frame
  void Update()
  {
    transform.position += velocity * Time.deltaTime;
  }
}
