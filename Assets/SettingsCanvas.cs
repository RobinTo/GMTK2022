using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsCanvas : MonoBehaviour
{
  [SerializeField]
  Slider masterVolumeSlider;
  [SerializeField]
  Slider musicVolumeSlider;
  [SerializeField]
  Slider sfxVolumeSlider;
  [SerializeField]
  Slider cameraMoveSensitivitySlider;

  void Start()
  {
    masterVolumeSlider.value = SettingsManager.instance.volume;
    musicVolumeSlider.value = SettingsManager.instance.musicVolume;
    sfxVolumeSlider.value = SettingsManager.instance.sfxVolume;
    cameraMoveSensitivitySlider.value = SettingsManager.instance.cameraMoveSensitivity;

    masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
    musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
    sfxVolumeSlider.onValueChanged.AddListener(SetSfxVolume);
    cameraMoveSensitivitySlider.onValueChanged.AddListener(SetCameraMoveSensitivity);
  }

  public void SetMasterVolume(float volume)
  {
    SettingsManager.instance.SetVolume(volume);
  }
  public void SetMusicVolume(float volume)
  {
    Debug.Log("Setting music volume to " + volume);
    SettingsManager.instance.SetMusicVolume(volume);
  }
  public void SetSfxVolume(float volume)
  {
    SettingsManager.instance.SetSFXVolume(volume);
  }
  public void SetCameraMoveSensitivity(float volume)
  {
    SettingsManager.instance.SetCameraMoveSensitivity(volume);
  }
}
