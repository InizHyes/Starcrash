using UnityEngine;
using UnityEngine.UI;

public class WeaponHUD : MonoBehaviour
{

    private int gunIndex;

    public Sprite weaponSprite1;
    public Sprite weaponSprite2;
    public Sprite weaponSprite3;
    public Sprite weaponSprite4;

    private Image weaponSpriteHolder;

    public GameObject character;

    
    void Awake()
    {
        weaponSpriteHolder = GetComponent<Image>();
    }

   // public void GetPlayerWeapons()

    
    void Update()
    {

        gunIndex = character.GetComponentInChildren<WeaponManager>().currentWeaponIndex;

            if (gunIndex == 0)
            {
                weaponSpriteHolder.sprite = weaponSprite1;
            }
            if (gunIndex == 1)
            {
                weaponSpriteHolder.sprite = weaponSprite2;
            }
            if (gunIndex == 2)
            {
                weaponSpriteHolder.sprite = weaponSprite3;
            }
            if (gunIndex == 3)
            {
                weaponSpriteHolder.sprite = weaponSprite4;
            }
        }
    }
