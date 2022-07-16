using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
  public static ObjectPooler instance;
  Dictionary<GameObject, List<GameObject>> pool = new Dictionary<GameObject, List<GameObject>>();

  public GameObject GetPooledObject(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
  {
    if (!pool.ContainsKey(prefab))
    {
      pool.Add(prefab, new List<GameObject>());
    }
    GameObject obj = null;
    if (pool[prefab].Count > 0)
    {
      obj = pool[prefab][0];
      pool[prefab].RemoveAt(0);
    }
    else
    {
      obj = Instantiate(prefab, position, rotation);
    }
    if (parent)
    {
      obj.transform.parent = parent;
    }
    obj.transform.position = position;
    obj.transform.rotation = rotation;
    obj.SetActive(true);
    return obj;
  }

  public void Release(GameObject prefab, GameObject obj)
  {
    if (!pool.ContainsKey(prefab))
    {
      pool.Add(prefab, new List<GameObject>());
    }
    obj.SetActive(false);
    pool[prefab].Add(obj);
  }


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
}
