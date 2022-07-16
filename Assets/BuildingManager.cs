using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
  public static BuildingManager instance;
  [SerializeField]
  SpriteRenderer preview;
  [SerializeField]
  LayerMask spaceshipLayer;

  bool building = false;
  GameObject buildingObject;

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

  void Start()
  {
    preview.gameObject.SetActive(false);
  }

  void Update()
  {
    if (!building) return;

    Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    preview.transform.position = new Vector3(worldPoint.x, worldPoint.y, 0);

    // Check if any 2d collider overlaps on the spaceshipLayer layermask
    Collider2D collision = Physics2D.OverlapCircle(preview.transform.position, 0.5f, spaceshipLayer);
    if (collision != null)
    {
      Debug.Log(collision.name);
      preview.color = Color.red;
    }
    else
    {
      preview.color = Color.green;
    }

    if (Input.GetMouseButtonDown(0))
    {
      if (collision == null)
      {
        Instantiate(buildingObject, preview.transform.position, Quaternion.identity);
        preview.gameObject.SetActive(false);
        building = false;
      }
    }
  }

  public void StartBuild(GameObject toBuild)
  {
    preview.gameObject.SetActive(true);
    building = true;
    buildingObject = toBuild;
  }
}
