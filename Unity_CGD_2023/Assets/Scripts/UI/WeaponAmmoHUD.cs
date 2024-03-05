using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class WeaponAmmoHUD : MonoBehaviour
{/*
    private TextMeshProUGUI ammocounter;
    public GameObject[] players;
    private shootingScript[] gunAmmo;

    private int gunIndex;
    private int ammoInGun;
    private int ammoMaxInGun;

    void Start()//grabs all the initial information so that the script doesn't wait for the player to do something
    {
        players = GameObject.FindGameObjectsWithTag("Player");//doesn't work. need to fetch player from scene, need to find way to differentiate

        ammocounter = GetComponent<TextMeshProUGUI>(); //fetches text component for updating with text

        for (int i = 0; i < players.Length; i++)
        {
            gunAmmo = players[i].GetComponentsInChildren<shootingScript>(); //grabs a list of player ammo

            ammoInGun = gunAmmo[gunIndex].ammoLoaded; //grabs current weapon's ammo
            ammoMaxInGun = gunAmmo[gunIndex].magSize; //grabs max mag size for current weapon

            ammocounter.text = (ammoInGun + " / " + ammoMaxInGun); //sets the text to what current ammo out of max mag size is
        }
    }

    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        ammocounter = GetComponent<TextMeshProUGUI>();

        for (int i = 0; i < players.Length; i++)
        {

            gunIndex = players[i].GetComponentInChildren<WeaponManager>().currentWeaponIndex;
            gunAmmo = players[i].GetComponentsInChildren<shootingScript>();

            Debug.Log(gunAmmo.Length);

            ammoInGun = gunAmmo[gunIndex].ammoLoaded;
            ammoMaxInGun = gunAmmo[gunIndex].magSize;

            ammocounter.text = ammoInGun.ToString() + " / " + ammoMaxInGun;
        }
    }*/
    private TextMeshProUGUI ammocounter;
    public GameObject[] players;
    private shootingScript[] gunAmmo;

    private int gunIndex;
    private int ammoInGun;
    private int ammoMaxInGun;

    void Start()//grabs all the initial information so that the script doesn't wait for the player to do something
    {
        players = GameObject.FindGameObjectsWithTag("Player");//doesn't work. need to fetch player from scene, need to find way to differentiate
        ammocounter = GetComponent<TextMeshProUGUI>(); //fetches text component for updating with text

        for (int i = 0; i < players.Length; i++)
        {
            gunAmmo = players[i].GetComponentsInChildren<shootingScript>(true); //grabs a list of player ammo
        }



        ammoInGun = gunAmmo[gunIndex].ammoLoaded; //grabs current weapon's ammo
        ammoMaxInGun = gunAmmo[gunIndex].magSize; //grabs max mag size for current weapon

        ammocounter.text = (ammoInGun + " / " + ammoMaxInGun); //sets the text to what current ammo out of max mag size is
    }

    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        ammocounter = GetComponent<TextMeshProUGUI>();

        for (int i = 0; i < players.Length; i++)
        {
            gunIndex = players[i].GetComponentInChildren<WeaponManager>().currentWeaponIndex;
            gunAmmo = players[i].GetComponentsInChildren<shootingScript>(true);
        }


        ammoInGun = gunAmmo[gunIndex].ammoLoaded;
        ammoMaxInGun = gunAmmo[gunIndex].magSize;

        ammocounter.text = ammoInGun.ToString(); //+ " / " + ammoMaxInGun);

    }
}
