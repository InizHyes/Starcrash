using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossWardenClass : EnemyClass{

    //This boss uses the guru's script as a base (unused code may be lying around).



    //public GameObject enemyPrefab; // Reference to the enemy prefab to spawn
    public Transform center; // ref to center
    public Transform topLeft; // Reference to a corner
    public Transform topRight; // Reference to a corner
    public Transform botLeft; // Reference to a corner
    public Transform botRight; // Reference to a corner

    private int timer = 0;
    private int atkcounter = 0;

    public GameObject reticlePrefab;

    public EnemyBossHealth_UI enemybosshealth_ui;
    public Image healthBar;
    public float healthAmount = 100f;

    private int movespeed = 5;
    private bool spin = true;

    // Array of destination points
    public Transform[] destinationPoints;
    private int currentDestinationIndex;

    private Animator anim;

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
        anim = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();

    }

    private void Update()
    {
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

                // Selects a random corner.
                int randomNum = Random.Range(1, 5);
                if (randomNum == lastnum)
                {
                    if (randomNum == 4)
                    {
                        randomNum = 1;
                    }
                    else
                    {
                        randomNum = randomNum + 1;
                    }
                }
                lastnum = randomNum;
                if (randomNum == 1)
                {
                    selectedPoint = topLeft;
                }
                else if (randomNum == 2)
                {
                    selectedPoint = topRight;
                }
                else if (randomNum == 3)
                {
                    selectedPoint = botLeft;
                }
                else if (randomNum == 4)
                {
                    selectedPoint = botRight;
                }
                originPoint = transform.position;
                enemyState = State.Moving;

                break;

            case State.Moving:
                /*
                * Move towards player with velocity
                * Will loop here until the state is changed back to Targeting, Attackng, or Dead
                */
                // Calculate the distance to the current destination
                destination = selectedPoint.transform.position;
                BossMove();

                // look at player
                //Vector3 direction = target.transform.position - transform.position;
                //transform.up = direction;
                break;

            case State.Attacking:
                if (atkcounter > 2)
                {
                    timer = timer + 1;
                    anim.SetBool("SwipeRight", false);
                    anim.SetBool("SwipeLeft", true);
                    anim.SetBool("IsMoving", false);
                    anim.SetBool("IsAttacking", false);
                    
                    targetClosestPlayer();
                    if (timer < 20)
                    {
                        transform.position = target.transform.position + new Vector3(0, 2, 0);
                        originPoint = transform.position;
                    }
                    else if (timer == 70)
                    {
                        destination = originPoint - new Vector3(0, 30, 0);
                    }
                    if ( timer == 90)
                    {
                        summonHitbox();
                    }
                    if (timer > 90)
                    {
                        spin = false;
                        movespeed = 20;
                        BossMove();
                        anim.SetBool("SwipeRight", true);
                        anim.SetBool("SwipeLeft", false);
                        anim.SetBool("IsMoving", false);
                        anim.SetBool("IsAttacking", false);
                    }
                    if (timer > 130)
                    {
                        movespeed = 5;
                        timer = 0;
                        atkcounter = 0;
                        enemyState = State.Targeting;
                        spin = true;
                    }

                }
                else
                {
                    timer = timer + 1;
                    if (timer == 5)
                    {
                        SpawnReticles();
                    }
                    else if (timer == 60)
                    {
                        SpawnReticles();
                    }
                    else if (timer > 100)
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
    private void SpawnReticles()
    {
        sound.clip = shootsound;
        sound.Play();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject playr in players)
            Instantiate(reticlePrefab, playr.transform.position, playr.transform.rotation);
    }

    protected void BossMove()
    {
        transform.position = Vector3.MoveTowards(originPoint, destination, lerpTime);
        lerpTime += movespeed * Time.deltaTime;
        if (spin)
        {
            transform.Rotate(0, 0, 20);
        }

        anim.SetBool("SwipeRight", false);
        anim.SetBool("SwipeLeft", false);
        anim.SetBool("IsMoving", true);
        anim.SetBool("IsAttacking", false);

        if (System.Math.Abs(transform.position.x - selectedPoint.transform.position.x) < 1 &&
             System.Math.Abs(transform.position.y - selectedPoint.transform.position.y) < 1)
        {
            anim.SetBool("SwipeRight", false);
            anim.SetBool("SwipeLeft", false);
            anim.SetBool("IsMoving", false);
            anim.SetBool("IsAttacking", true);
            enemyState = State.Attacking;
            transform.rotation = Quaternion.identity;
            lerpTime = 0;
        }
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

    private void summonHitbox()
    {
        // Load the hitbox
        GameObject hitboxPrefab = Resources.Load<GameObject>("SuperHitBox");

        if (hitboxPrefab != null)
        {
            // Instantiate the hitbox prefab
            GameObject hitboxInstance = Instantiate(hitboxPrefab, transform.position, Quaternion.identity);

            hitboxInstance.transform.parent = transform; // used to make it a child (hitbox sticks to the entity)
            // Access the Hitbox script on the instance to set its variables
            SuperHitboxScript hitboxScript = hitboxInstance.GetComponent<SuperHitboxScript>();

            if (hitboxScript != null)
            {
                // Set relevant variable information for the hitbox (IMPORTANT)
                hitboxScript.damageAmount = 50; //HELLO TO ANYONE NERFING THIS GUY THIS IS HIS STAB ATTACK DMG
                hitboxScript.size = new Vector2(0.6f, 1f); // these numbers need to be very small
                hitboxScript.rotationAngle = transform.eulerAngles.z;
                hitboxScript.offsetAmount = new Vector2(0f, 0f);
                hitboxScript.lifetime = 0.55f;
                hitboxScript.deleteOnConnect = false; // make sure this is true
            }
            else
            {
                Debug.LogError("brokey");
            }
        }
        else
        {
            Debug.LogError("dont worke");
        }
    }
}
