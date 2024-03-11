using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyBossHealth_UI : MonoBehaviour
{
    // Sets the variables 
    public Image healthBar;
    public Image ShieldBar;
    public float healthAmount = 1500f;

    public GameObject boss;
    //public float ShieldAmount = 10;

    // Update is called once per frame
    void Update()
    {
        //if(GameObject.FindGameObjectWithTag("Player"))
        //{
            //TakeDamage(1);
        //}
        // If a key is pressed
        // The health bar decreases
        // If a key is pressed
        // The health bar increases
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
            //Heal(5);
        //}

        if (healthBar.fillAmount == 0)
        {
            print("BOSS DEAD");
            boss.SetActive(false);

            GameObject doorManagerObject = GameObject.Find("DoorManager");
            if (doorManagerObject != null)
            {
                DoorManager doorManager = doorManagerObject.GetComponent<DoorManager>();

                doorManager.OpenDoors();

                SceneManager.LoadScene("Win"); // Load the win screen scene
            }
        }
    }

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / 1500f;
        //ShieldAmount -= damage;
        //ShieldBar.fillAmount = ShieldAmount / 10f;
    }

    public void Heal(float healingAmount)
    {
        healthAmount += healingAmount;
        // Sets the min and maz value to 0 and 100
        healthAmount = Mathf.Clamp(healthAmount, 0, 1500);

        healthBar.fillAmount = healthAmount / 1500;

        //ShieldAmount += healingAmount;
        // Sets the min and maz value to 0 and 100
        //ShieldAmount = Mathf.Clamp(ShieldAmount, 0, 10);

        //ShieldBar.fillAmount = ShieldAmount / 10f;
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if(collision.gameObject.tag == "Bullet")
        {
            TakeDamage(5);
        }
    }
}
