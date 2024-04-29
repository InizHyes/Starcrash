using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    public PlayerStats playerStats; // Assuming PlayerStats is a script
    public GameObject emptySlider; /// refrence to the empoty health bar so sprite can be changed per player
    public Sprite sliderSprite; /// refrence of slider sprite so it can be changed back
    public Sprite downedSprite; /// will be changed to this when downed

    public Image playerIcon;
    public Image playerBar;
    public Image gunImage;
    public TMP_Text playerText;
    public TMP_Text gunText;


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
                //emptySlider.GetComponent<Image>().sprite = downedSprite;
                playerIcon.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);
                playerBar.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);
                gunImage.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);
                playerText.GetComponent<TMP_Text>().color = new Color(1, 1, 1, 0.1f);
                gunText.GetComponent<TMP_Text>().color = new Color(1, 1, 1, 0.1f);

            }
            else if (playerStats.health > 0 ) 
            {
                emptySlider.GetComponent<Image>().sprite = sliderSprite;
                playerIcon.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                playerBar.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                gunImage.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                playerText.GetComponent<TMP_Text>().color = new Color(1, 1, 1, 1);
                gunText.GetComponent<TMP_Text>().color = new Color(1, 1, 1, 1);
            }
        }
    }
}
