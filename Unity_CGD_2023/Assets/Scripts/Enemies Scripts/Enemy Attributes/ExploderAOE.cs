using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploderAOE : MonoBehaviour
{
    /*
     * On activate flash 4 times
     * Linger and explode on 5th
     * If player in radius deal damage to them
     * Set Exploder to dead - exploder will linger for a few seconds
     */

    // Dont deal damage on trigger until 5th flash
    private bool dealDamage;

    private void Start()
    {
        dealDamage = false;
    }

    private void LateUpdate()
    {
        
    }
}
