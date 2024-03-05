using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    public int totalWeapons = 1;
    public int currentWeaponIndex;

    //PlayerController swapInputs;

    Player swapButtons;

    public GameObject[] weapons;
    public GameObject weaponHolder;
    public GameObject currentWeapon;

    public Transform player;

    public shootingScript GunScript;

    private smokeBehaviour gunSmoke;

    public float pickUpRange;
    public float dropForce;

    public bool equipped;
    public static bool slotFull;

    private bool previousWeapon;

    // Start is called before the first frame update
    void Start()
    {
        //swapInputs = GetComponentInParent<PlayerController>();

        swapButtons = GetComponentInParent<Player>();

        totalWeapons = weaponHolder.transform.childCount;
        weapons = new GameObject[totalWeapons];

        for (int i = 0; i < totalWeapons; i++)
        {
            weapons[i] = weaponHolder.transform.GetChild(i).gameObject;
            weapons[i].SetActive(false);
        }
        weapons[0].SetActive(true);
        currentWeapon = weapons[0];
        currentWeaponIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        

        Swap();
        /*
        Vector2 distanceToPlayer = player.position - transform.position;
        if(!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.F) && !slotFull) //pick up weapon if F is pressed
        {
            PickUp();
        }

        if(Input.GetKeyDown(KeyCode.G))
        {
            GunScript = weapons[currentWeaponIndex].GetComponent<shootingScript>();
            Drop();
        }*/
    }

    /*
    private void PickUp()
    {
       // transform.SetParent(weaponHolder);
    }

    private void Drop()
    {
        weapons[currentWeaponIndex].RemoveFromHeirachy();
        weapons[currentWeaponIndex].AddComponent<Rigidbody2D>();
        weapons[currentWeaponIndex].AddComponent<Collider2D>();

        weapons[currentWeaponIndex].transform.SetParent(null);

        weapons[currentWeaponIndex].GetComponent<Rigidbody>().isKinematic = false;
        weapons[currentWeaponIndex].GetComponent<Collider2D>().isTrigger = false;

        weapons[currentWeaponIndex].GetComponent<Rigidbody2D>().AddForce(player.forward);

        GunScript.enabled = false;
        
        if (previousWeapon)
        {
            currentWeaponIndex -= 1;
            weapons[currentWeaponIndex].SetActive(true);
            previousWeapon = false;
        }
        else if (!previousWeapon)
        {
            currentWeaponIndex += 1;
            weapons[currentWeaponIndex].SetActive(true);
            previousWeapon = true;
        }
    }
    */
    private void Swap()
    {
        if (swapButtons.swapRightTriggered)
        {
            if (currentWeaponIndex < totalWeapons - 1)
            {
                weapons[currentWeaponIndex].SetActive(false);
                weapons[currentWeaponIndex].GetComponentInChildren<smokeBehaviour>().resetSpriteFrame();
                currentWeaponIndex += 1;
                swapButtons.swapRightTriggered = false;
                weapons[currentWeaponIndex].SetActive(true);
                previousWeapon = true;
            }
            else if (currentWeaponIndex == totalWeapons - 1)
            {
                weapons[currentWeaponIndex].SetActive(false);
                weapons[currentWeaponIndex].GetComponentInChildren<smokeBehaviour>().resetSpriteFrame();
                currentWeaponIndex = 0;
                swapButtons.swapRightTriggered = false;
                weapons[currentWeaponIndex].SetActive(true);
                previousWeapon = true;
            }
        }
        if (swapButtons.swapLeftTriggered)
        {
            if (currentWeaponIndex > 0)
            {
                weapons[currentWeaponIndex].SetActive(false);
                weapons[currentWeaponIndex].GetComponentInChildren<smokeBehaviour>().resetSpriteFrame();
                currentWeaponIndex -= 1;
                swapButtons.swapLeftTriggered = false;
                weapons[currentWeaponIndex].SetActive(true);
                previousWeapon = false;
            }

            else if (currentWeaponIndex == 0)
            {
                weapons[currentWeaponIndex].SetActive(false);
                weapons[currentWeaponIndex].GetComponentInChildren<smokeBehaviour>().resetSpriteFrame();
                currentWeaponIndex = totalWeapons - 1;
                swapButtons.swapLeftTriggered = false;
                weapons[currentWeaponIndex].SetActive(true);
                previousWeapon = false;
            }
        }

        /*
        if (swapInputs.player == 1)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (currentWeaponIndex < totalWeapons - 1)
                {
                    weapons[currentWeaponIndex].SetActive(false);
                    currentWeaponIndex += 1;
                    weapons[currentWeaponIndex].SetActive(true);
                    previousWeapon = true;
                }
                else if (currentWeaponIndex == totalWeapons - 1)
                {
                    weapons[currentWeaponIndex].SetActive(false);
                    currentWeaponIndex = 0;
                    weapons[currentWeaponIndex].SetActive(true);
                    previousWeapon = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (currentWeaponIndex > 0)
                {
                    weapons[currentWeaponIndex].SetActive(false);
                    currentWeaponIndex -= 1;
                    weapons[currentWeaponIndex].SetActive(true);
                    previousWeapon = false;
                }

                else if (currentWeaponIndex == 0)
                {
                    weapons[currentWeaponIndex].SetActive(false);
                    currentWeaponIndex = totalWeapons - 1;
                    weapons[currentWeaponIndex].SetActive(true);
                    previousWeapon = false;
                }
            }
        }

        if (swapInputs.player == 2)
        {
            if (swapInputs.swapForwardTriggered)
            {
                if (currentWeaponIndex < totalWeapons - 1)
                {
                    weapons[currentWeaponIndex].SetActive(false);
                    currentWeaponIndex += 1;
                    swapInputs.swapForwardTriggered = false;
                    weapons[currentWeaponIndex].SetActive(true);
                    previousWeapon = true;
                }
                else if (currentWeaponIndex == totalWeapons - 1)
                {
                    weapons[currentWeaponIndex].SetActive(false);
                    currentWeaponIndex = 0;
                    swapInputs.swapForwardTriggered = false;
                    weapons[currentWeaponIndex].SetActive(true);
                    previousWeapon = true;
                }
            }
            if (swapInputs.swapBackTriggered)
            {
                if (currentWeaponIndex > 0)
                {
                    weapons[currentWeaponIndex].SetActive(false);
                    currentWeaponIndex -= 1;
                    swapInputs.swapBackTriggered = false;
                    weapons[currentWeaponIndex].SetActive(true);
                    previousWeapon = false;
                }

                else if (currentWeaponIndex == 0)
                {
                    weapons[currentWeaponIndex].SetActive(false);
                    currentWeaponIndex = totalWeapons - 1;
                    swapInputs.swapBackTriggered = false;
                    weapons[currentWeaponIndex].SetActive(true);
                    previousWeapon = false;
                }
            }
        }
        */
    }
}
