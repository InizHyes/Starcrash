using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DEVPlayerUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI enemyCountText;

    [Header("Attributes")]
    [SerializeField] private PlayerStats player;
    [SerializeField] private SpawnLogic spawnController;

    private void Update()
    {
        // Player health
        healthText.text = "Health: " + player.health;

        // Gun ammo
        foreach (shootingScript gun in player.GetComponentInChildren<WeaponManager>().GetComponentsInChildren<shootingScript>())
        {
            if (gun.isActiveAndEnabled)
            {
                ammoText.text = "Ammo: " + gun.ammoLoaded + " / " + gun.magSize;
            }
        }

        // Enemy reserves
        if (spawnController != null)
        {
            enemyCountText.text = "Enemies: " + (spawnController.nPCCounter + spawnController.nPCTotal);
        }
    }
}
