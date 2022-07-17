using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
  public static AudioManager instance;

  List<AudioSource> sfxSources = new List<AudioSource>();

  public Action<float> OnMusicVolumeChanged;
  public Action<float> OnSFXVolumeChanged;

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

  public void SetMusicVolume(float volume)
  {
    AudioSource musicSource = GetComponent<AudioSource>();
    musicSource.volume = volume;
    OnMusicVolumeChanged?.Invoke(volume);
  }

  public void SetSFXVolume(float volume)
  {
    OnSFXVolumeChanged?.Invoke(volume);
  }
}
