using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player: MonoBehaviour
{
    private PlayerInput playerInput;
    PlayerManager playerManager;

    public Rigidbody2D rb;
  /*  [SerializeField] private float thrustForce = 1f;*/
    [SerializeField] private float rotationSpeed = 5f;


    [SerializeField] private float shootForce = 5f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float damping = 1f;
    public PhysicsMaterial2D bounceMaterial;

    private Vector2 moveInput;
    private Vector2 lookInput;
    bool shooting = false;
    bool reloading = false; //maintaining consistency of implementation for ease of understanding

    public Vector2 shootDirection;

    //New Gunscript implementation
    shootingScript GunScript;
    private bool swapRightButton = false;
    private bool swapLeftButton = false;
    public bool swapRightTriggered = false;
    public bool swapLeftTriggered = false;
    public bool reloadTriggered = false;

    private void Awake()
    {
        playerInput = this.GetComponent<PlayerInput>();
        //pauseMenu = FindObjectOfType<PauseMenu>();
        playerManager = FindObjectOfType<PlayerManager>();
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        playerInput.actions["Move"].performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInput.actions["Look"].performed += ctx => lookInput = ctx.ReadValue<Vector2>();

        playerInput.actions["Attack"].started += i => StartShoot();
        playerInput.actions["Attack"].canceled += i => EndShoot();
        playerInput.actions["Reload"].started += i => Reload();

        playerInput.actions["WeaponSwapLeft"].started += i => WeaponSwapLeft();
        playerInput.actions["WeaponSwapRight"].started += i => WeaponSwapRight();
        playerInput.actions["Lockdown"].performed += i => Lockdown();

        playerInput.actions["Pause"].performed += i => playerManager.Pause();
        playerInput.actions["Resume"].performed += i => playerManager.Resume();
    }

    private void OnDisable()
    {

    }

    void Update()
    {
        HandleInput();

        GunScript = GetComponentInChildren<shootingScript>();

        shootDirection = lookInput.normalized;

        GunScript.Shoot(shooting, reloading);
    }

    void HandleInput()
    {
        if (lookInput.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(lookInput.y, lookInput.x) * Mathf.Rad2Deg;
            rb.rotation = Mathf.LerpAngle(rb.rotation, angle, rotationSpeed * Time.deltaTime);
        }
    }
    private void FixedUpdate()
    {


        if (reloading)
        {
            reloadTriggered = true;
            reloading = false;
        }

        if (shooting)
        {
            Shoot();
        }

        if(swapLeftButton)
        {
            swapLeftTriggered = true;
            swapLeftButton = false;
        }
        if(swapRightButton)
        {
            swapRightTriggered = true;
            swapRightButton = false;
        }

    }
   
    private void StartShoot()
    {
        shooting = true;

        

        // Apply recoil force to move the player backward
        //rb.AddForce(-shootDirection * shootForce, ForceMode2D.Impulse);
    }

    private void EndShoot()
    {
        shooting = false;
    }

    private void Shoot()
    {
        // Calculate shoot direction based on look input
        

        

        // Apply damping force to reduce the velocity gradually
        Vector2 dampingForce = -rb.velocity * damping;
        rb.AddForce(dampingForce);

        // Limit the maximum speed in both directions
        Vector2 clampedVelocity = new Vector2(
            Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed),
            Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed)
        );

        rb.velocity = clampedVelocity;
    }

    private void Reload()
    {
        // Reload Weapon
        reloading = true;
    }

    private void WeaponSwapLeft()
    {
        // Toggle weapon left
        swapLeftButton = true;
    }
    private void WeaponSwapRight()
    {
        // Toggle weapon right
        swapRightButton = true;
    }
    private void Lockdown()
    {
        // Toggle clamp
    }
}
