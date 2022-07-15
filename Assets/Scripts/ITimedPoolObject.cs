using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITimedPoolObject
{
    void ReturnAfter(GameObject prefab, float time);
}
