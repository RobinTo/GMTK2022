using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

public enum Resource
{
  Wood,
  Iron,
  Gold,
  Diamond,
  CircuitBoard,
  AdvancedCircuitBoard,
  Wires
}

public class ResourcePanel : MonoBehaviour
{
  [SerializeField]
  ImageAndTextUIElement resourcePrefab;

  Dictionary<Resource, ImageAndTextUIElement> resourceDisplays;


  void Start()
  {
    resourceDisplays = new Dictionary<Resource, ImageAndTextUIElement>();
    ResourceManager.instance.OnResourceChanged += OnResourceChanged;
  }

  void OnResourceChanged(Resource resource, int amount)
  {
    if (!resourceDisplays.ContainsKey(resource))
      AddResource(resource);
    resourceDisplays[resource].SetText(amount.ToString());
  }

  public void AddResource(Resource resource)
  {
    ImageAndTextUIElement element = Instantiate(resourcePrefab, transform);
    element.SetImage(ResourceManager.GetSprite(resource));
    element.SetText("0");
    resourceDisplays.Add(resource, element);
  }
}
