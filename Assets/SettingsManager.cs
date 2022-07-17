using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
  public static SettingsManager instance;

  public float volume = 1f;
  public float musicVolume = 1f;
  public float sfxVolume = 1f;

  public float cameraMoveSensitivity = 1f;

  void Awake()
  {
    if (instance == null)
    {
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }

  public void SetVolume(float volume)
  {
    this.volume = volume;
    AudioListener.volume = volume;
  }

  public void SetMusicVolume(float volume)
  {
    this.musicVolume = volume;
    AudioManager.instance.SetMusicVolume(volume);
  }

  public void SetSFXVolume(float volume)
  {
    this.sfxVolume = volume;
    AudioManager.instance.SetSFXVolume(volume);
  }

  public void SetCameraMoveSensitivity(float sensitivity)
  {
    this.cameraMoveSensitivity = sensitivity;
  }
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }
}
