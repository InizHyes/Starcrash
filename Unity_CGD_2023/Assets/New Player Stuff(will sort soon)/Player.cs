using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player: MonoBehaviour
{
    private PlayerInput playerInput;
    PlayerManager playerManager;
    Lockdown lockdownScript;



    private Rigidbody2D rb;
  /*  [SerializeField] private float thrustForce = 1f;*/
    [SerializeField] private float rotationSpeed = 5f;

    [SerializeField] public float shootForce = 5f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float damping = 1f;
    public PhysicsMaterial2D bounceMaterial;

    private Vector2 moveInput;
    private Vector2 lookInput;
    bool shooting = false;

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
        lockdownScript = GetComponent<Lockdown>();
    }

    private void OnEnable()
    {
        playerInput.actions["Move"].performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInput.actions["Look"].performed += ctx => lookInput = ctx.ReadValue<Vector2>();

        playerInput.actions["Attack"].performed += i => StartShoot();
        playerInput.actions["Reload"].performed += i => Reload();

        playerInput.actions["WeaponSwapLeft"].performed += i => WeaponSwapLeft();
        playerInput.actions["WeaponSwapRight"].performed += i => WeaponSwapRight();
        playerInput.actions["Lockdown"].performed += i => Lockdown();

        playerInput.actions["Pause"].performed += i => playerManager.Pause();
        playerInput.actions["Resume"].performed += i => playerManager.Resume();
    }

    private void OnDisable()
    {

    }
    public bool IsShooting
    {
        get { return shooting; }
    }

    void Update()
    {
        HandleInput();
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
        if (shooting)
        {
            Shoot();
        }
    }
   
    private void StartShoot()
    {
        shooting = true;

        // Calculate shoot direction based on look input
        Vector2 shootDirection = lookInput.normalized;

        // Apply recoil force to move the player backward
        rb.AddForce(-shootDirection * shootForce, ForceMode2D.Impulse);
    }

    private void Shoot()
    {
        // Apply damping force to reduce the velocity gradually
        Vector2 dampingForce = -rb.velocity * damping;
        rb.AddForce(dampingForce);

        // Limit the maximum speed in both directions
        Vector2 clampedVelocity = new Vector2(
            Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed),
            Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed)
        );
        rb.velocity = clampedVelocity;

        if (!shooting)
            return;
    }

    private void Reload()
    {
        // Reload Weapon
    }

    private void WeaponSwapLeft()
    {
        // Toggle weapon left
    }
    private void WeaponSwapRight()
    {
        // Toggle weapon right
    }
    private void Lockdown()
    {
          lockdownScript.Locked();
    }
}
