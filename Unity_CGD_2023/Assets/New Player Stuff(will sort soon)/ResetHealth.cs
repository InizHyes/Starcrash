using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetHealth : MonoBehaviour
{
    public PlayerStats player;
    public TeleportOnCollision tp;

    public void Awake()
    {
        tp = GetComponent<TeleportOnCollision>();
    }

    public void UpdateHealth()
    {
        if (tp.roomChange == true)
        {
            player.health = player.maxHealth;
            Debug.Log("HEALTH");
        }
    }
}
