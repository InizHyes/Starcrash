using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAilment : MonoBehaviour
{
    private PlayerStats stats;

    // Ailments
    [Header("Poison")]
    [SerializeField][Tooltip("How long poison is applied. In seconds.")] private float poisonDuration = 5f;
    [SerializeField][Tooltip("How often poison deals damage. In seconds.")] private float poisonTickSpeed = 1f;
    [SerializeField][Tooltip("Poison damage per tick.")] private int poisonDamage = 1;
    private bool poisoned = false;
    private float poisonDurationCounter = 0f;
    private float poisonTickSpeedCounter = 1f;

    /*
     * -----For other devs-----
     * If you want to do something when an ailment is applied or removed,
     * ctrl + f to find the "\o/"
     * And enter required code below
     * 
     * ---Ailment list---
     * 0 = Poison
     */

    void Start()
    {
        stats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        AilmentCheck();
    }

    public void InvokeAilment(int id)
    {
        /*
         * Sets ailment bool to true based on id
         * 
         * 0 = Poison
         */

        switch (id)
        {
            case 0:
                // ---Poison---
                if (!poisoned)
                {
                    // -----If you want to do somthing on ailment start, do it here-----
                    // \o/ Poison start

                    poisoned = true;
                    poisonTickSpeedCounter = poisonTickSpeed;
                }
                // Always reset duration
                poisonDurationCounter = poisonDuration;

                break;
        }
    }

    private void AilmentCheck()
    {
        // ---Poison---
        if (poisoned)
        {
            // Check poision duration, if < 0 end poison
            if (poisonDurationCounter > 0)
            {
                poisonDurationCounter -= Time.deltaTime;

                // Check tick duration, if < 0 deal damage
                if (poisonTickSpeedCounter > 0)
                {
                    poisonTickSpeedCounter -= Time.deltaTime;
                }
                else
                {
                    // Deal damage and reset timer
                    stats.TakeDamage(poisonDamage);
                    poisonTickSpeedCounter = poisonTickSpeed;
                }
            }
            else
            {
                EndAilment(0);
            }
        }
    }

    private void EndAilment(int id)
    {
        /*
         * Sets ailment bool to false based on id
         * 
         * 0 = Poison
         */

        switch (id)
        {
            case 0:
                // ---Poison---
                // -----If you want to do somthing on ailment end, do it here-----
                // \o/ Poison end

                poisoned = false;
                poisonDurationCounter = 0;
                poisonTickSpeedCounter = 0;
                break;
        }
    }
}
