using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBossHealth_UI : MonoBehaviour
{
    // Sets the variables 
    public Image healthBar;
    public float healthAmount = 100f;

    // Update is called once per frame
    void Update()
    {
        if(GameObject.FindGameObjectWithTag("Enemy"))
        // If a key is pressed
        // The health bar decreases
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TakeDamage(20);
        }
        // If a key is pressed
        // The health bar increases
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Heal(5);
        }
    }

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / 100f;
    }

    public void Heal(float healingAmount)
    {
        healthAmount += healingAmount;
        // Sets the min and maz value to 0 and 100
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);

        healthBar.fillAmount = healthAmount / 100f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "BulletCollision")
        {
            TakeDamage(10);

        }
    }
}
