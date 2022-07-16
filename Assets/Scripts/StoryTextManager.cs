using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StoryTextManager : MonoBehaviour
{
  int currentStep = 0;
  string welcomeMessage = "Good morning Captain. You have been asleep for 7 years, space ship status is: mostly ruined.";
  string welcomeMessage2 = "We currently have enough resources to build a module. I suggest buying a mining module to try establishing a supply of resources.";
  string welcomeMessage3 = "You can access information and spend resources to upgrade your modules by clicking on them. Upgrading them will increase their resilience.";
  string welcomeMessage4 = "We should gather resources for our next modules. Lets try saving up for a shield and rocket module to get some defenses up and running.";

  float timer = 0;


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
  }

  void Update()
  {
    timer += Time.deltaTime;

    if (currentStep == 1 && timer > 15)
    {
      currentStep = 2;
      TextPanel.instance.ShowText(welcomeMessage4);
      TextPanel.instance.onTextFinished = () =>
      {
        TextPanel.instance.Hide();
        currentStep = 3;
        timer = 0;
        EnemySpawner.instance.SetActive(true);
      };
    }
  }
}
