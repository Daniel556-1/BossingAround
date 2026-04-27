using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusic : MonoBehaviour
{
    public AudioSource musicSource;
    // Start is called before the first frame update
    void Start()
    {
        if (SettingsBridge.Instance != null)
        {
            musicSource.volume = SettingsBridge.Instance.musicVolume;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
