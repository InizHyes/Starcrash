using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
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
    public Camera view;
    public GameObject otherPlayer; //For vertical slice

    public float SpeedCap = 10;
    public int player = 0;
    [SerializeField] private InputActionReference movement, attack, rotate;
    private bool shoot = false;
    [SerializeField] PlayerInput playerinput;


    /// Bullet work - Arch
    [SerializeField]
    private Transform gunPoint;

    [SerializeField]
    private GameObject bullet;



    
    private void OnEnable() ///handles the inputs, buttons only get detected like this
    {
        attack.action.performed += AttackPressed;
        
    }
    private void OnDisable() ///disables the function when the button is released
    {
        attack.action.performed -= AttackPressed;
    }

    private void AttackPressed(InputAction.CallbackContext context) ///makes shoot true which makes the chcrater shoot in update
    {
        shoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        rb.velocity = MoveForce2; ///actually where movment happen
        if (player == 1) ///KEYBOARD CONTROLLS
        {
            Vector2 PlayerInput = new Vector2(Input.GetAxisRaw("HorizontalKeyboard"), Input.GetAxisRaw("VerticalKeyboard")).normalized; ///gets playerinput
            Vector2 MoveForce = PlayerInput * MoveSpeed; ///applies the speed to player input
            MoveForce2 = MoveForce2 + PlayerInput * MoveSpeed; ///adding the players input onto the current move vector 2, never loses momentum as it is adding rather than setting
            MoveForce2 += ForceToApply; ///force to apply is instant force, e.g if you wanted to fire a gun (below in mouse section) change apply force to alter current movement
            ForceToApply /= ForceDamping; ///so you arent just constantly adding the same force
            rb.velocity = MoveForce2; ///actually where movment happen
            Vector2 MouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;  ///these three lines make the player look at the mouse
            var angle = Mathf.Atan2(MouseDir.y, MouseDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);




            if (Input.GetMouseButtonDown(0)) ///this function checks where the mouse is clicked and applies force to the player in the opposite direction
            {
                print("GunFired");
                MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                ///print(MousePos);
                PlayerPos = transform.position;
                ///print(PlayerPos);
                ForceDir = (MousePos - PlayerPos).normalized;
                ///print(ForceDir);
                ForceToApply = (ForceDir * GunForce * -1.0f); ///change gunforce to change knockback effect
                                                              ///rb.AddForce(ForceDir * GunForce * -1.0f);
                FireBullet();
            }

        }
        else if (player == 2) ///CONTROLLER CONTROLS
        {
           
            Vector2 PlayerInput = movement.action.ReadValue<Vector2>();
            Vector2 MoveForce = PlayerInput * MoveSpeed; ///applies the speed to player input
            MoveForce2 = MoveForce2 + PlayerInput * MoveSpeed; ///adding the players input onto the current move vector 2, never loses momentum as it is adding rather than setting
            MoveForce2 += ForceToApply; ///force to apply is instant force, e.g if you wanted to fire a gun (below in mouse section) change apply force to alter current movement
            ForceToApply /= ForceDamping; ///so you arent just constantly adding the same force
            rb.velocity = MoveForce2; ///actually where movment happen
            Vector2 RightStick = rotate.action.ReadValue<Vector2>();

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
                FireBullet();
                shoot = false;
                                                              
                    
            }





        }




    }

    private void FireBullet() // called every time fire is pressed - Arch
    {
        GameObject firedBullet = Instantiate(bullet, gunPoint.position, gunPoint.rotation); //creates an instance of bullet at the position of the "gun" - Arch
        firedBullet.GetComponent<Rigidbody2D>().velocity = gunPoint.up * 10f; //adds force to the bullet - Arch
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
            print("MOOOVE FUCKING CAMERA");

            Vector2 playerLocation = transform.position;
            playerLocation.x = playerLocation.x - 0.1f;
            otherPlayer.transform.position = playerLocation;
        }
    }
}