using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairModule : MonoBehaviour
{
  SpaceShipModule spaceshipModule;


  [SerializeField]
  float repairInterval = 15f;
  float timer = 0;

  [SerializeField]
  float repairChance = 10;
  [SerializeField]
  int repairAmount = 1;

  // Start is called before the first frame update
  void Start()
  {
    spaceshipModule = GetComponent<SpaceShipModule>();
  }

  // Update is called once per frame
  void Update()
  {
    timer += Time.deltaTime;
    if (timer < repairInterval) return;
    // Repair one health on a connected module that has lost health if random is less than repair chance
    if (spaceshipModule.health < spaceshipModule.maxHealth && Random.Range(0, 100) < repairChance)
    {
      spaceshipModule.health += repairAmount;
      timer = 0;
    }
    foreach (SpaceShipModule module in spaceshipModule.ConnectedModules)
    {
      if (module.health < module.maxHealth && Random.Range(0, 100) < repairChance)
      {
        module.health += repairAmount;
        timer = 0;
        break;
      }
    }

  }
}
