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
        healthText.text = "Health: " + StatsManager.Instance.currentHealth + " / " + StatsManager.Instance.maxHealth;

        StatsManager.Instance.currentHealth = StatsManager.Instance.maxHealth;
        healthSlider.maxValue = StatsManager.Instance.maxHealth;
        healthSlider.value = StatsManager.Instance.currentHealth;                   
    }
    public void ChangeHealth(int amount)
    {
        StatsManager.Instance.currentHealth += amount;
        healthSlider.value = StatsManager.Instance.currentHealth;
        healthText.text = "Health: " + StatsManager.Instance.currentHealth + " / " + StatsManager.Instance  .maxHealth;


        if (StatsManager.Instance.currentHealth <= 0)
        {
            gameObject.SetActive(false);
            healthSlider.fillRect.gameObject.SetActive(false);
        }
    }
}