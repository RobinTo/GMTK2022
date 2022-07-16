using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpgradable
{
    void Upgrade();
    List<ResourceCost> GetCost();
}
