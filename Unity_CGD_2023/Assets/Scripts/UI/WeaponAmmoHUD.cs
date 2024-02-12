using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponAmmoHUD : MonoBehaviour
{
    private TextMeshProUGUI ammocounter;
    public GameObject player;
    private shootingScript[] gunAmmo;

    private int gunIndex;
    private int ammoInGun;
    private int ammoMaxInGun;

    void Start()//grabs all the initial information so that the script doesn't wait for the player to do something
    {
        player = GameObject.FindWithTag("Player").gameObject;//doesn't work. need to fetch player from scene, need to find way to differentiate
        ammocounter = GetComponent<TextMeshProUGUI>(); //fetches text component for updating with text
        
        gunAmmo = player.GetComponentsInChildren<shootingScript>(true); //grabs a list of player ammo

        ammoInGun = gunAmmo[gunIndex].ammoLoaded; //grabs current weapon's ammo
        ammoMaxInGun = gunAmmo[gunIndex].magSize; //grabs max mag size for current weapon

        ammocounter.text = (ammoInGun + " / " + ammoMaxInGun); //sets the text to what current ammo out of max mag size is
    }

    void Update()
    {
        ammocounter = GetComponent<TextMeshProUGUI>();
        gunIndex = player.GetComponentInChildren<WeaponManager>().currentWeaponIndex;
        gunAmmo = player.GetComponentsInChildren<shootingScript>(true);

        ammoInGun = gunAmmo[gunIndex].ammoLoaded;
        ammoMaxInGun = gunAmmo[gunIndex].magSize;

        ammocounter.text = (ammoInGun + " / " + ammoMaxInGun);

    }
}
