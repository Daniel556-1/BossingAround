using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsBridge : MonoBehaviour
{
    public static SettingsBridge Instance;

    public float musicVolume = 1f;
    public float sfxVolume = 1f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateMusic(float volume)
    {
        musicVolume = volume;
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    }

    public void UpdateSFX(float volume)
    {
        sfxVolume = volume;
        PlayerPrefs.SetFloat("SfxVolume", sfxVolume);
    }
}
