using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasDamage : MonoBehaviour
{
    private float timeSpentInGas;
    public float timeBeforeDamage;
    public float damageInterval;
    private float damageIntervalTimer;
    public int damage;


    private void OnTriggerStay2D(Collider2D collision)
    {
        //IF PLAYER STAYS IN GAS FOR MORE THAN timeBeforeDamage PLAYER STARTS TAKING damage IN PRESET damageIntervals
        if (collision.tag == "Player")
        {
            timeSpentInGas += Time.deltaTime;
            if (timeSpentInGas >= timeBeforeDamage)
            {
                damageIntervalTimer += Time.deltaTime;
                if(damageIntervalTimer >= damageInterval)
                {
                    damageIntervalTimer = 0;

                    //CHANGES HP IN PLAYER SCRIPT IF SOMEONE ADDS DAMAGE SYSTEM CHANGE SCRIPT AND VARIABLE NAMES
                    collision.GetComponent<PlayerStats>().health = collision.GetComponent<PlayerStats>().health - damage;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            timeSpentInGas = 0;
            damageIntervalTimer = 0;
        }
    }
}
