using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    
    public Slider healthSlider;
    public TMP_Text healthText;


    void Start()
    {
        if (StatsManager.Instance == null)
        {
            return;
        }

        StatsManager.Instance.currentHealth = Mathf.Clamp(
            StatsManager.Instance.currentHealth,
            0,
            StatsManager.Instance.maxHealth);
        UpdateHealthUI();
    }
    public void ChangeHealth(int amount)
    {
        if (StatsManager.Instance == null)
        {
            return;
        }

        StatsManager.Instance.currentHealth = Mathf.Clamp(
            StatsManager.Instance.currentHealth + amount,
            0,
            StatsManager.Instance.maxHealth);
        UpdateHealthUI();

        if (StatsManager.Instance.currentHealth == 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = StatsManager.Instance.maxHealth;
            healthSlider.value = StatsManager.Instance.currentHealth;
        }

        if (healthText != null)
        {
            healthText.text = "Health: " + StatsManager.Instance.currentHealth + " / " + StatsManager.Instance.maxHealth;
        }
    }
}