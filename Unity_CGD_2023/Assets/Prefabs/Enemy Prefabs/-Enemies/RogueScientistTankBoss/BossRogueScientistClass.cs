using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossRogueScientistClass : EnemyClass{

    //This boss uses the guru's script as a base (unused code may be lying around).



    //public GameObject enemyPrefab; // Reference to the enemy prefab to spawn
    public Transform center; // ref to center
    public Transform topLeft; // Reference to a corner
    public Transform topRight; // Reference to a corner
    public Transform botLeft; // Reference to a corner
    public Transform botRight; // Reference to a corner

    public GameObject FrontTurretLeft;
    public GameObject FrontTurretRight;
    public GameObject LeftTurret;
    public GameObject RightTurret;
    public GameObject BackTurret;

    public GameObject slowRazor;
    public GameObject fastRazor;
    public GameObject RazorPrefab;

    private int timer = 0;
    private int atkcounter = 0;

    public GameObject reticlePrefab;

    public EnemyBossHealth_UI enemybosshealth_ui;
    public Image healthBar;
    public float healthAmount = 1000f;

    private int movespeed = 5;
    private bool spin = false;
    private bool lookingAtPlayer = true;
    private float spinamount = 1;
    private int waitint = 0;


    private Transform selectedPoint;
    private Vector3 originPoint;
    private Vector3 destination;
    protected float lerpTime;
    private int lastnum = 10;
    public AudioClip shootsound;

    AudioSource sound;

    private void Start()
    {
        // Set starting state and variables
        initiateEnemy();
        sound = GetComponent<AudioSource>();

    }

    private void Update()
    {
        if (spin)
        {
            transform.Rotate(0, 0, spinamount * Time.deltaTime);
            if (spinamount < 800)
            {
                spinamount = spinamount + 5;
            }
        }
        else
        {
            spinamount = 1;
        }

        if (target != null)
        {
            if (!spin)
            {
                if (LeftTurret.GetComponent<BossLaserSniperClass>().attackTimer < 101)
                {
                    if (waitint < 1)
                    {
                        Vector3 direction = target.transform.position - transform.position; // look at player
                        transform.up = -direction;
                    }
                    else
                    {
                        waitint = waitint - 1;
                    }
                }
                else
                {
                    waitint = 10;
                }
                    
            }
        }
        if (slowRazor == null)
        {
            GameObject razorInstance = Instantiate(RazorPrefab, LeftTurret.transform.position, LeftTurret.transform.rotation);
            EnergyRazorBlade razorScript = razorInstance.GetComponent<EnergyRazorBlade>();
            slowRazor = razorInstance;
            if (LeftTurret.transform.position.y > RightTurret.transform.position.y)
            {
                razorScript.direction = "up";
            }
            else
            {
                razorScript.direction = "down";
            }
            razorScript.speed2 = 6f;
        }
        if (fastRazor == null)
        {
            GameObject razorInstance = Instantiate(RazorPrefab, RightTurret.transform.position, RightTurret.transform.rotation);
            EnergyRazorBlade razorScript = razorInstance.GetComponent<EnergyRazorBlade>();
            fastRazor = razorInstance;
            if (LeftTurret.transform.position.y > RightTurret.transform.position.y)
            {
                razorScript.direction = "down";
            }
            else
            {
                razorScript.direction = "up";
            }
            razorScript.speed2 = 8f;
        }
        Debug.Log(enemyState);
        switch (enemyState)
        {
            case State.Initiating:
                /*
                 * Starting state, used to run one-off functions for spawning
                 */

                
                targetClosestPlayer();
                enemyState = State.Targeting;
                break;


            case State.Targeting:
                targetClosestPlayer();
                enemyState = State.Attacking;
                break;

            case State.Moving:
                break;

            case State.Attacking:
                if (atkcounter > 2)
                {
                    spin = true;
                    timer = timer + 1;
                    if (timer == 600)
                    {
                        ShootTurrets();
                    }
                    if (timer == 1000)
                    {
                        ShootTurrets();
                    }
                    if (timer > 1400)
                    {
                        timer = 0;
                        atkcounter = 0;
                        enemyState = State.Targeting;
                        spin = false;
                    }

                }
                else
                {
                    timer = timer + 1;
                    if (timer == 5)
                    {
                        ShootTurrets();
                    }
                    else if (timer > 500)
                    {
                        timer = 0;
                        enemyState = State.Targeting;
                        atkcounter += 1;
                    }
                }

                    break;

            case State.Dead:
                /*
                 * Runs item drop logic then runs the logic associated with the enemy leaving the scene
                 * Can run death animation before running these functions
                 */

                //itemDropLogic();

                initiateDeath();

                // Find the GameObject with the DoorManager script attached
                GameObject doorManagerObject = GameObject.Find("DoorManager");
                if (doorManagerObject != null)
                {
                    DoorManager doorManager = doorManagerObject.GetComponent<DoorManager>();

                    doorManager.OpenDoors();

                    //print("All enemies dead");
                }

                break;
        }
    }

    private void ShootTurrets()
    {
        LeftTurret.GetComponent<BossLaserSniperClass>().changestate(4);
        RightTurret.GetComponent<BossLaserSniperClass>().changestate(4);
        FrontTurretLeft.GetComponent<BossLaserSniperClass>().changestate(4);
        FrontTurretRight.GetComponent<BossLaserSniperClass>().changestate(4);
        BackTurret.GetComponent<BossLaserSniperClass>().changestate(4);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            // Change the enemy color or apply the animation here
            ChangeEnemyColor();
        }
    }

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / health;
    }

    private void ChangeEnemyColor()
    {
        // Implement logic to change the enemy color or apply the animation here
        // Change the sprite renderer color
       GetComponent<SpriteRenderer>().color = Color.red;

        // Alternatively, trigger an animation
        //anim.SetTrigger("Hit");

        StartCoroutine(ResetHitState());
    }

    private IEnumerator ResetHitState()
    {
        // Wait for a short duration before resetting the hit state
        yield return new WaitForSeconds(0.1f);
        // Reset the "Hit" trigger
        //anim.ResetTrigger("Hit");

        GetComponent<SpriteRenderer>().color = Color.white;
    }
}