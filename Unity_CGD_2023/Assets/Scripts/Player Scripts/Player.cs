using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{ /// some variables below may be uselss, was testing many things
    public Rigidbody2D rb;
    public float MoveSpeed = 100;
    public float GunForce = 5;
    public Vector2 ForceToApply;
    public float ForceDamping;
    Vector2 MoveForce2;
    Vector2 MousePos;
    Vector2 PlayerPos;
    Vector2 ForceDir;
    Vector2 LastVel;
    Vector2 RightStickOld;
    Vector2 noMove = new Vector2(0, 0);
    public bool sticking = false;
    public Camera view;
    public GameObject otherPlayer; //For vertical slice

    public bool interactInput;

    public float SpeedCap = 10;
    public int player = 0;
    [SerializeField] private InputActionReference movement, attack, rotate, stickToSurface;
    private bool shoot = false;

    [SerializeField] public PlayerInput playerinput;
    private InputActionAsset inputAsset;
    public InputActionMap playerControl;
    public InputActionMap menuControl;
    private InputAction move;
    private InputAction look;

    PauseMenu pauseMenu;

    /// Bullet work again, whilst very angry due to github eating all my work, apologies if it doesn't work yet - Arch
    shootingScript shooting;

    private void Awake()
    {
        inputAsset = this.GetComponent<PlayerInput>().actions;  ///these pieces of code identify the players unique inputs, allowing for multiple controllers
        playerControl = inputAsset.FindActionMap("PlayerControls");
        menuControl = inputAsset.FindActionMap("MenuControls"); /// only janky thing about it is the "" for each control map variable name, but it works!
        pauseMenu = FindObjectOfType<PauseMenu>();
    }


    private void OnEnable() ///handles the inputs, buttons only get detected like this
    {
        playerControl.FindAction("attack").started += AttackPressed;  ///using this as an example, when the action is "started" (pressed) it calls the function that does the thing
        move = playerControl.FindAction("Move");  ///assigns the unique controllers move and look (once again part oif what allows multiple controllers)
        look = playerControl.FindAction("Look");
        playerControl.FindAction("Lockdown").started += stickingToSurface;
        playerControl.FindAction("Pause").started += i => Pause();
        menuControl.FindAction("Resume").started += i => Resume();


    }

    private void OnDisable() ///disables the function when the button is released
    {
        playerControl.FindAction("attack").started -= AttackPressed;   ///this disables the function almost immediatly, so when it is pressed it only happens once
        stickToSurface.action.performed -= stickingToSurface;
        playerControl.FindAction("Lockdown").started -= stickingToSurface;
        playerControl.FindAction("Pause").started -= i => Pause();
        menuControl.FindAction("Resume").started -= i => Resume();


    }
    private void Pause()
    {
        playerinput.SwitchCurrentActionMap("MenuControls");
        pauseMenu.Pause();
    }

    public void Resume()
    {
        playerinput.SwitchCurrentActionMap("PlayerControls");
        pauseMenu.ResumeGame();
    }
    private void AttackPressed(InputAction.CallbackContext context) ///makes shoot true which makes the chcrater shoot in update
    {
        shoot = true;
        
        
    }

    private void stickingToSurface(InputAction.CallbackContext context) ///used to make the player "stick" to the ground. Starts timer to initilise 
    {
        StartCoroutine(stickTimer2());
        
        

    }

    IEnumerator stickTimer2() ///this start a timer to tick to ground then flips the variable. So it takes a second to stick and unstick, as a drawback 
    {
        yield return new WaitForSeconds(1);
        sticking = !sticking;
        

    }





    // Update is called once per frame
    void Update()
    {
        shooting = GetComponentInChildren<shootingScript>();
        shooting.shoot(player, shoot);

        if (!sticking)
        {
            rb.velocity = MoveForce2; ///actually where movment happen
        }
        else
        {
            print("no move");
            rb.velocity = noMove;
        }

        if (player == 1) ///THIS WHOLE BIT IS OLD CODE, I WILL REMOVE IT WHEN TWO CONTOLLERS WORK.
        {
            Vector2 PlayerInput = new Vector2(Input.GetAxisRaw("HorizontalKeyboard"), Input.GetAxisRaw("VerticalKeyboard")).normalized; ///gets playerinput
            Vector2 MoveForce = PlayerInput * MoveSpeed; ///applies the speed to player input
            MoveForce2 = MoveForce2 + PlayerInput * MoveSpeed; ///adding the players input onto the current move vector 2, never loses momentum as it is adding rather than setting
            MoveForce2 += ForceToApply; ///force to apply is instant force, e.g if you wanted to fire a gun (below in mouse section) change apply force to alter current movement
            ForceToApply /= ForceDamping; ///so you arent just constantly adding the same force
            Vector2 MouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;  ///these three lines make the player look at the mouse
            var angle = Mathf.Atan2(MouseDir.y, MouseDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);




            if (Input.GetMouseButtonDown(0)) ///this function checks where the mouse is clicked and applies force to the player in the opposite direction
            {
                MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                ///print(MousePos);
                PlayerPos = transform.position;
                ///print(PlayerPos);
                ForceDir = (MousePos - PlayerPos).normalized;
                ///print(ForceDir);
                ForceToApply = (ForceDir * GunForce * -1.0f); ///change gunforce to change knockback effect
            }

        }
        else if (player == 2) ///CONTROLLER CONTROLS
        {

           
            Vector2 PlayerInput = move.ReadValue<Vector2>(); ///reading the specific controllers movement
            Vector2 MoveForce = PlayerInput * MoveSpeed; ///applies the speed to player input
            if (!sticking) ///if the player is not supposed to be sticking to the ground, do movement normally
            {
                rb.velocity = MoveForce2; ///actually where movment happen
                MoveForce2 = MoveForce2 + PlayerInput * MoveSpeed; ///adding the players input onto the current move vector 2, never loses momentum as it is adding rather than setting
                MoveForce2 += ForceToApply; ///force to apply is instant force, e.g if you wanted to fire a gun (below in mouse section) change apply force to alter current movement
                ForceToApply /= ForceDamping; ///so you arent just constantly adding the same force
            }
            else ///else if the player is supposed to be sticking, stop all momentum and momentum gain
            {
                print("no move");
                rb.velocity = noMove;
                MoveForce2 = noMove;
            }

            Vector2 RightStick = look.ReadValue<Vector2>();

            if (RightStick != RightStickOld)
            {
                var angle = Mathf.Atan2(RightStick.y, RightStick.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            if (shoot == true)  ///this function checks if shoot is true, then "shoots"
            {
                print("GunFired");
                MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                ForceDir = transform.right;
                ForceToApply = (ForceDir * GunForce * -1.0f); ///change gunforce to change knockback effect
                shoot = false;
                                                              
                    
            }






        }




    }


    private void OnCollisionEnter2D(Collision2D collision)
    { ///this whole section does collision, its buggy as hell but it gets the job done for now as proof of concept

        Vector2 CollDir = (collision.transform.position - transform.position).normalized;
        if (collision.transform.position.y > transform.position.y)
        {
            MoveForce2.y = (CollDir.y * 1 * -1.0f);
        }
        if (collision.transform.position.y < transform.position.y)
        {
            MoveForce2.y = (CollDir.y * 1 * -1.0f);
         
        }
        if (collision.transform.position.x < transform.position.x)
        {
            MoveForce2.x = (CollDir.x * 1 * -1.0f);

        }
        if (collision.transform.position.x > transform.position.x)
        {
            MoveForce2.x = (CollDir.x * 1 * -1.0f);

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Floor"))
        {
            Vector3 floorLocation = collision.gameObject.transform.position;
            floorLocation.z = -10;
            view.transform.position = floorLocation;

            Vector2 playerLocation = transform.position;
            playerLocation.x = playerLocation.x - 0.1f;
            otherPlayer.transform.position = playerLocation;
        }
    }
}