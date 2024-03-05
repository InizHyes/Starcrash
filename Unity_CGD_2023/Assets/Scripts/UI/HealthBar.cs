using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    public PlayerStats playerStats; // Assuming PlayerStats is a script

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        if (playerStats != null)
        {
            // Access the health and maxHealth values from the PlayerStats script
            float currentHealth = playerStats.health;
            float maxHealth = playerStats.maxHealth;

            // Update the slider value based on the player's health
            float fillValue = currentHealth / maxHealth;
            slider.value = fillValue;
        }
    }
}
