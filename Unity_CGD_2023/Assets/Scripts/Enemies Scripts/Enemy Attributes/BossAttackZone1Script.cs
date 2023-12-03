using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackZone1Script : MonoBehaviour
{
    private BossClass parentScript;

    private void Start()
    {
        parentScript = GetComponent<BossClass>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If trigger is player
        if (collision != null && collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerStats>().TakeDamage(parentScript.attack1Damage);
        }
    }
}
