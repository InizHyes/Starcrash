using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    //public TextMeshProUGUI healthCounter;
    public GameObject playerStats;

    private float currentHealth, maxHealth;
    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player");
        currentHealth = playerStats.GetComponent<PlayerStats>().health;
        maxHealth = playerStats.GetComponent<PlayerStats>().maxHealth;

        float fillValue = currentHealth / maxHealth;
        slider.value = fillValue;

        //healthCounter.text = currentHealth + " / " + maxHealth;
    }
}
