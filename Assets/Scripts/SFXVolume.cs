using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXVolume : MonoBehaviour
{
    public AudioSource sfxSource;
    // Start is called before the first frame update
    void Start()
    {
        if (SettingsBridge.Instance != null)
        {
            sfxSource.volume = SettingsBridge.Instance.sfxVolume;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
