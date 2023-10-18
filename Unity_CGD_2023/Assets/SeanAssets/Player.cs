using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{ /// some variables below may be uselss, was testing many things
    public Rigidbody2D rb;
    public float MoveSpeed = 100;
    public float GunForce = 10;
    public Vector2 ForceToApply;
    public float ForceDamping;
    Vector2 MoveForce2;
    Vector2 MousePos;
    Vector2 PlayerPos;
    Vector2 ForceDir;
    Vector2 LastVel;
    public float SpeedCap = 10;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 PlayerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized; ///gets playerinput
        Vector2 MoveForce = PlayerInput * MoveSpeed; ///applies the speed to player input
        MoveForce2 = MoveForce2 + PlayerInput * MoveSpeed; ///adding the players input onto the current move vector 2, never loses momentum as it is adding rather than setting
        MoveForce2 += ForceToApply; ///force to apply is instant force, e.g if you wanted to fire a gun (below in mouse section) change apply force to alter current movement
        ForceToApply /= ForceDamping; ///so you arent just constantly adding the same force
        rb.velocity = MoveForce2; ///actually where movment happens

        Vector2 MouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        var angle =Mathf.Atan2(MouseDir.y, MouseDir.x) * Mathf.Rad2Deg;
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


        print("collision");
        ///i cant figure out how to make the player bounce of walls effectivly
        ///right now, the player will hit a wall but their velocity can keep increasing, so if you try accelerate in a different direction it will take a second because of the build up
        ///solution to this is to chnge the force to another direction when hitting the wall (bouncing off, even a little) but i cant figure it out rn
        ///might need to do something with raycasts, or a jank solution with one collide box for each side, and then depending on which side is hit push them that way proportinal to 
      
    }
}