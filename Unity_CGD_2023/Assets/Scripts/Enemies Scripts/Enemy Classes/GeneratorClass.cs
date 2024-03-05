using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GeneratorClass : EnemyClass
{
    private Animator animator;

    private void Start()
    {
        // Set starting state and variables

        health = 25;

        animator = GetComponent<Animator>();

        // Disable the Animator component to stop playing the death animation
        animator.enabled = false;

        initiateEnemy();
    }

    private void Update()
    {
        switch (enemyState)
        {
            case State.Initiating:
                /*
                 * Starting state, used to run one-off functions for spawning
                 */

                if (health <= 0 || (Input.GetKeyDown(KeyCode.Backspace)))
                {
                    enemyState = State.Dead;
                }

                break;

            case State.Dead:
                /*
                 * Runs item drop logic then runs the logic associated with the enemy leaving the scene
                 * Can run death animation before running these functions
                 */

                // Make sure death animation plays before enemy destruction 
                StartCoroutine(WaitForDeathAnimation());

                break;
        }
    }

    private IEnumerator WaitForDeathAnimation()
    {

        // Enable the Animator component to play the death animation
        animator.enabled = true;

        // Wait for one frame to ensure that the animation has started
        yield return null;

        // Get the length of the current animation, 
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;

        // Wait for the duration of the enemy death animation
        yield return new WaitForSeconds(animationLength);

        //Now the enemy dies after animation is done.
        animator.enabled = false;
        itemDropLogic();
        initiateDeath();
        StopCoroutine(WaitForDeathAnimation());
    }
}
