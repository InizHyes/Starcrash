using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class WeaponAmmoHUD : MonoBehaviour
{/*
    private TextMeshProUGUI ammocounter;
    public GameObject character;
    private shootingScript gunAmmo;

    private int ammoInGun;
    //private int ammoMaxInGun;

    private void Awake()
    {
        ammocounter = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        gunAmmo = character.GetComponentInChildren<shootingScript>();

        ammoInGun = gunAmmo.ammoLoaded;
        //ammoMaxInGun = gunAmmo.magSize;

        ammocounter.text = (ammoInGun.ToString())/* + " / " + ammoMaxInGun)*/;

    }
}
