using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleSODatabase : MonoBehaviour
{
    public static ModuleSODatabase instance;

    public List<ModuleSO> moduleSOs;


    void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public ModuleSO GetModuleSO(BuildingId buildingId) {
        foreach(ModuleSO moduleSO in moduleSOs) {
            if(moduleSO.buildingId == buildingId) {
                return moduleSO;
            }
        }
        return null;
    }
}
