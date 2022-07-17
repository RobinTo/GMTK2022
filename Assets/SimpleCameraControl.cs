using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraControl : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    Vector2 velocity = Vector2.zero;
    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
    {
      velocity.y = 1;
    }
    if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
    {
      velocity.y = -1;
    }
    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
    {
      velocity.x = -1;
    }
    if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
    {
      velocity.x = 1;
    }
    transform.position += new Vector3(velocity.x, velocity.y, 0) * Time.deltaTime;
  }
}
