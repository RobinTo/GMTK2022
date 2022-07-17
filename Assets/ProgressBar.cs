using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
  Camera main;
  float offset = 1;

  [SerializeField]
  Image fillImage;

  GameObject attached;
  // Start is called before the first frame update
  void Start()
  {
    main = Camera.main;
  }

  public void Attach(GameObject gameObject, float offset)
  {
    this.attached = gameObject;
    this.offset = offset;
  }

  // Update is called once per frame
  void Update()
  {
    transform.position = main.WorldToScreenPoint(attached.transform.position + Vector3.down * offset);
  }

  public void SetProgress(float progress)
  {
    fillImage.fillAmount = progress;
  }
}
