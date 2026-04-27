using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public Slider healthBar;
    public TextMeshProUGUI healthText;

    public AudioClip hurtClip;
    public AudioClip deathClip;
    public AudioSource SFXPlayer;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();

        if (SettingsBridge.Instance != null)
        {
            SFXPlayer.volume = SettingsBridge.Instance.sfxVolume;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        UpdateUI();

        if (currentHealth <= 0)
        {
            SFXPlayer.clip = deathClip;
            SFXPlayer.loop = true;
            SFXPlayer.Play();
            Die();
        } else
        {
            SFXPlayer.clip = hurtClip;
            SFXPlayer.Play();
        }
    }

    void UpdateUI()
    {
        if (healthBar != null)
        {
            healthBar.value = Mathf.Clamp(currentHealth / maxHealth, 0f, 1f);
        }

        if (healthText != null)
        {
            healthText.text = currentHealth + "/" + maxHealth;
        }
    }

    void Die()
    {
        if (gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Dead");

        } else if (gameObject.CompareTag("Boss"))
        {
            Debug.Log("Boss Dead");
        } else
        {
            Debug.Log("Homing missile dead");
        }
    }
}
