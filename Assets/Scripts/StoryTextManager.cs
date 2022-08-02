using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StoryTextManager : MonoBehaviour
{
  int currentStep = 0;
  string welcomeMessage = "Good morning Captain. You have been asleep for 7 years, space ship status: mostly ruined.";
  string welcomeMessage2 = "We currently have enough resources to build a module. I suggest buying a mining module to try establishing a supply of resources.";
  string welcomeMessage3 = "You can access information and spend resources to upgrade your modules by clicking on them. Upgrading them will increase their effectiveness. Consider upgrading your miner.";
  string welcomeMessage4 = "I found the schematics for a shield and rocket module, lets try to get some defenses up and running.";
  string welcomeMessage5 = "We looted some materials and schematics from that ship. Check out the new buildings in the building menu.";
  string connectionInfo = "Some modules such as the repair module requires a connection to modules to repair them. Each module can have 3 connections, build connections from the building menu by clicking the modules.";
  string bossMessage = "Wow, that ship contained some rare schematics, take a look at our new modules in your building menu!";

  float timer = 0;

  bool firstEnemyDestroyed = false;

  [SerializeField]
  GameObject countdowmGO;

  void Start()
  {
    TextPanel.instance.ShowText(welcomeMessage);
    TextPanel.instance.onTextFinished = () =>
    {
      TextPanel.instance.ShowText(welcomeMessage2);
      TextPanel.instance.onTextFinished = () =>
      {
        TextPanel.instance.ShowText(welcomeMessage3);
        TextPanel.instance.onTextFinished = () =>
        {
          TextPanel.instance.Hide();
          currentStep = 1;
          timer = 0;
        };
      };
    };
    Enemy.OnEnemyDestroyed += (Enemy e) =>
    {
      if (e.isBoss)
      {
        ShowNextBossMessage();
        return;
      }
      if (firstEnemyDestroyed) return;

      ResourceManager.instance.ModifyResource(Resource.CircuitBoard, 2);
      BuildingManager.instance.UnlockBuilding(BuildingId.CircuitProducer);
      BuildingManager.instance.UnlockBuilding(BuildingId.Repair);
      firstEnemyDestroyed = true;
      TextPanel.instance.ShowText(welcomeMessage5);
      TextPanel.instance.onTextFinished = () =>
      {
        TextPanel.instance.ShowText(connectionInfo);
        TextPanel.instance.onTextFinished = () =>
        {
          TextPanel.instance.Hide();
          currentStep = 4;
          timer = 0;
        };
      };
    };
  }

  void Update()
  {
    timer += Time.deltaTime;

    if (currentStep == 1 && timer > 15)
    {
      currentStep = 2;
      TextPanel.instance.ShowText(welcomeMessage4);
      BuildingManager.instance.UnlockBuilding(BuildingId.Shield);
      BuildingManager.instance.UnlockBuilding(BuildingId.Rocket);
      TextPanel.instance.onFullTextShowing = () =>
      {
        EnemySpawner.instance.SetActive(true);
        countdowmGO.SetActive(true);
        TextPanel.instance.onFullTextShowing = () => { };
      };
      TextPanel.instance.onTextFinished = () =>
      {
        TextPanel.instance.Hide();
        currentStep = 3;
        timer = 0;
      };
    }

    if (currentStep == 4 && timer > 60)
    {
      EnemySpawner.instance.TriggerBossSpawn(30);
      currentStep = 5;
    }
  }

  void ShowNextBossMessage()
  {
    TextPanel.instance.ShowText(bossMessage);
    AsteroidsSpawner.instance.IncreaseChancesAfterBoss();
    BuildingManager.instance.UnlockBuilding(BuildingId.AdvancedCircuitProducer);
    BuildingManager.instance.UnlockBuilding(BuildingId.HomingRocket);
    BuildingManager.instance.UnlockBuilding(BuildingId.PassiveMiner);
    TextPanel.instance.onTextFinished = () =>
    {
      TextPanel.instance.Hide();
      timer = 0;
    };
  }
}
