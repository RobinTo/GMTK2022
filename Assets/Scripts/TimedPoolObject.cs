using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedPoolObject : MonoBehaviour, ITimedPoolObject
{
  float timer = 0;
  GameObject prefab;

  public void ReturnAfter(GameObject prefab, float time)
  {
    this.prefab = prefab;
    timer = time;
  }

  void Update()
  {
    if (!prefab) return;

    timer -= Time.deltaTime;
    if (timer <= 0)
    {
      ObjectPooler.instance.Release(prefab, gameObject);
    }
  }
}
