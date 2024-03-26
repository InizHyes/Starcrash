using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    public PlayerStats playerStats; // Assuming PlayerStats is a script
    public GameObject emptySlider; /// refrence to the empoty health bar so sprite can be changed per player
    public Sprite sliderSprite; /// refrence of slider sprite so it can be changed back
    public Sprite downedSprite; /// will be changed to this when downed


    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        if (playerStats != null)
        {
            // Access the health and maxHealth values from the PlayerStats script
            float currentHealth = playerStats.health;
            float maxHealth = playerStats.maxHealth;

            // Update the slider value based on the player's health
            float fillValue = currentHealth / maxHealth;
            slider.value = fillValue;

            /// Sean - this just changes the healthbar empty sprite to be DOWN in all caps, to change the image assign downSprite in the editor
            if (playerStats.health <= 0 )
            {
                emptySlider.GetComponent<Image>().sprite = downedSprite;

            }
            else if (playerStats.health > 0 ) 
            {
                emptySlider.GetComponent<Image>().sprite = sliderSprite;
            }
        }
    }
}
