using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StraightShooterClass : EnemyClass
{

    [Header("BulletPrefab")]
    public GameObject bulletPrefab;
    [Header("BulletSpeed")]
    public float bulletSpeed = 5f;
    [Header("BulletCooldown")]
    public float shootInterval = 2f; // Adjust this to control the time between shots
    [Header("BulletLifetime")]
    public float bulletLifetime = 3f; // Adjust this to set the lifetime of the bullets
    [Header("BulletsPerBurst")]
    public int bulletsPerBurst = 5;      // Number of bullets to fire in each burst
    [Header("TimeBetweenBursts")]
    public float burstPause = 3f;        // Adjust this to set the pause between bursts

    private int bulletsFired;
    private bool isInBurst;
    private Animator anim;


    private void Start()
    {
        // Set starting state and variables
        initiateEnemy();
        StartBurst();  // Start the first burst
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        switch (enemyState)
        {
            case State.Initiating:
                /*
                 * Starting state, used to run one-off functions for spawning
                 */

                //enemyState = State.Targeting;
                break;

            case State.Targeting:
                /*
                 * Target player and decide if State.Pathfinding is needed, otherwise change to moving
                 */

                //targetClosestPlayer();
                //enemyState = State.Moving;
                break;

            case State.Pathfinding:
                /*
                 * Pathfind if line of sight is blocked
                 */

                break;

            case State.Moving:
                /*
                * Move towards player with velocity
                * Will loop here until the state is changed back to Targeting, Attackng, or Dead
                */

                //moveTowardsTarget0G();

                // look at player
                /*Vector3 direction = target.transform.position - transform.position;
                transform.up = direction;*/
                break;

            case State.Attacking:
                /*
                 * Change State to here after attack is used
                 * Will wait here until attackCooldown is over then move back to Targeting
                 * 
                 * Before setting state to State.Attacking run //attackCooldownValue = attackCooldown;
                 * This will set the attackCooldownValue so that attackCooldwonLogic() can count it down
                 */

                // Count-down timer
                /*if (attackCooldwonLogic())
                {
                    enemyState = State.Targeting;
                }*/

                break;

            case State.Dead:
                /*
                 * Runs item drop logic then runs the logic associated with the enemy leaving the scene
                 * Can run death animation before running these functions
                 */

                itemDropLogic();
                initiateDeath();
                break;
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

    void StartBurst()
    {
        if (!isInBurst)
        {
            isInBurst = true;
            bulletsFired = 0;
            InvokeRepeating("ShootBullet", 0f, shootInterval);
            Invoke("EndBurst", bulletsPerBurst * shootInterval);
        }
    }

    void EndBurst()
    {
        CancelInvoke("ShootBullet");
        Invoke("StartBurst", burstPause);
        isInBurst = false;
        anim.SetBool("Shoot", false);
    }

    void ShootBullet()
    {
        anim.SetBool("Shoot", true);
        if (bulletsFired < bulletsPerBurst)
        {
            SpawnBullet(Vector2.up);
            SpawnBullet(Vector2.down);
            SpawnBullet(Vector2.left);
            SpawnBullet(Vector2.right);

            bulletsFired++;
        }
    }

    void SpawnBullet(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * bulletSpeed;
        Destroy(bullet, bulletLifetime);
    }

    private void ChangeEnemyColor()
    {
        // Implement logic to change the enemy color or apply the animation here
        // Change the sprite renderer color
        // GetComponent<SpriteRenderer>().color = Color.red;

        // Alternatively, trigger an animation
        anim.SetTrigger("Hit");

        StartCoroutine(ResetHitState());
    }

    private IEnumerator ResetHitState()
    {
        // Wait for a short duration before resetting the hit state
        yield return new WaitForSeconds(0.1f);
        // Reset the "Hit" trigger
        anim.ResetTrigger("Hit");

        GetComponent<SpriteRenderer>().color = Color.white;
    }
}