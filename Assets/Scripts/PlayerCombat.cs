using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public GameObject swordPrefab;
    public Transform spawnPoint;
    public Camera camera;
    public float fireCooldown = 2f;
    public float fireTimer;
    public StateManager stateManager;
    public GameObject tempObjects;

    public AudioClip throwClip;
    public AudioSource SFXPlayer;

    public float damageMultiplier = 1f;

    // Update is called once per frame
    void Update()
    {
        if (fireTimer > 0)
        {
            fireTimer -= Time.deltaTime;
        }

        if (SettingsBridge.Instance != null)
        {
            SFXPlayer.volume = SettingsBridge.Instance.sfxVolume;
        }

        if (Input.GetButtonDown("Fire1") && fireTimer <= 0 && stateManager != null && stateManager.currentState == "Fight" && stateManager.isPaused == false)
        {
            SFXPlayer.clip = throwClip;
            SFXPlayer.Play();
            ThrowSword();
            fireTimer = fireCooldown;
        }
    }

    void ThrowSword()
    {
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 target;

        target = ray.GetPoint(100);
        
        Vector3 direction = (target - spawnPoint.position).normalized;
        GameObject sword = Instantiate(swordPrefab, spawnPoint.position, Quaternion.LookRotation(direction));
        sword.transform.parent = tempObjects.transform;
        SwordThrow swordScript = sword.GetComponent<SwordThrow>();
        swordScript.damage *= damageMultiplier;
    }
}
