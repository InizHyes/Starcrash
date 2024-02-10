using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHUD : MonoBehaviour
{

    private int gunIndex;

    public Sprite weaponSprite1;
    public Sprite weaponSprite2;
    public Sprite weaponSprite3;
    public Sprite weaponSprite4;

    public GameObject Player;
    private Image weaponSpriteHolder;

    // Start is called before the first frame update
    void Awake()
    {
        weaponSpriteHolder = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        gunIndex = Player.GetComponent<WeaponManager>().currentWeaponIndex;

        if(gunIndex == 0)
        {
            weaponSpriteHolder.sprite = weaponSprite1;
        }
        else if (gunIndex == 1)
        {
            weaponSpriteHolder.sprite = weaponSprite2;
        }
        else if (gunIndex == 2)
        {
            weaponSpriteHolder.sprite = weaponSprite3;
        }
        else if (gunIndex == 3)
        {
            weaponSpriteHolder.sprite = weaponSprite4;
        }
    }

}
