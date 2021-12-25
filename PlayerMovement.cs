using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Based on and modified from Plai's youtube tutorial on Rigidbody FPS Movement, https://www.youtube.com/watch?v=LqnPeqoJRFY&list=PLRiqz5jhNfSo-Fjsx3vv2kvYbxUDMBZ0u, https://github.com/Plai-Dev/rigidbody-fps-controller-tutorial
public class PlayerMovement : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;

    public HealthBar healthBar;

    Animator animator;
    private bool canDash;
    private bool canSwing;
    public float playerHeight = 2f;
    public float groundAirResistRatio = 3f;
    public Transform orientation;
    private WallRun wallRunScript;
    public PlayerLook playerLookScript;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float airMultiplier = 0.4f;
    private float movementMultiplier = 10f;

    [Header("Sprinting")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 6f;
    public float acceleration = 10f;

    [Header("Jumping")]
    public float jumpForce = 5f;
    public float gravity = -3f;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Drag")]
    public float groundDrag = 6f;
    public float airDrag = 2f;

    private float horizontalMovement;
    private float verticalMovement;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public LayerMask groundMask;
    public LayerMask abyssMask;
    public float groundDistance = 0.2f;
    public bool isGrounded { get; private set; }
    public bool isFallen { get; private set; }

    private Vector3 moveDirection;
    private Vector3 slopeMoveDirection;

    private Rigidbody rb;

    private RaycastHit slopeHit;

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        wallRunScript = GetComponent<WallRun>();
        rb.freezeRotation = true;
        canDash = true;
        isFallen = false;
        animator = GetComponentInChildren<Animator>();
        canSwing = true;
        
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isFallen = Physics.CheckSphere(groundCheck.position, groundDistance, abyssMask);
        if (isGrounded)
        {
            animator.SetBool("isOnGround", true);
        } else
        {
            animator.SetBool("isOnGround", false);
        }
        MyInput();
        ControlDrag();
        ControlSpeed();

        if (isFallen)
        {
            SoundManager.S.MakePlayerDeathSound();
            rb.isKinematic = true;
            StartCoroutine(waitToDie());
            SceneManager.LoadScene("GameOverScreen");
        }
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }
        if (Input.GetKeyDown(sprintKey) && canDash)
        {
            Dash();
            StartCoroutine(DashCooldown());   
        }
        if (Input.GetButtonDown("Fire1"))
        {
            if(canSwing)
            {
                Swing();
                StartCoroutine(SwingCooldown());
            }
            
        }
        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

        if (currentHealth < maxHealth)
        {
            currentHealth += 5 * Time.deltaTime;
            healthBar.SetHealth(currentHealth);
        }
        
    }

    private void Swing()
    {
        SoundManager.S.MakeAirSwingSound();
        animator.SetTrigger("isSlash");
    }

    private IEnumerator DashCooldown()
    {
        canDash = false;
        yield return new WaitForSeconds(0.5f);
        canDash = true;
    }

    private IEnumerator SwingCooldown()
    {
        canSwing = false;
        yield return new WaitForSeconds(0.5f);
        canSwing = true;
    }
    private void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
        if (horizontalMovement == 0 && verticalMovement == 0)
        {
            animator.SetBool("isRunning", false);
        } else
        {
            animator.SetBool("isRunning", true);
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            SoundManager.S.MakePlayerJumpSound();
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        } 
    }

    private void Dash()
    {
        SoundManager.S.MakeDashSound();
        if (moveDirection != new Vector3(0, 0, 0)) { rb.velocity = new Vector3(0, 0, 0); }
        if (isGrounded)
        {
            if (playerLookScript.xRotation < -45f)
            {
                rb.AddForce(transform.up.normalized * (sprintSpeed / 1.5f), ForceMode.Impulse);
                rb.AddForce(moveDirection.normalized * (sprintSpeed / 1.5f), ForceMode.Impulse);
            }
            else if (playerLookScript.xRotation > 45f)
            {
                rb.AddForce(transform.up.normalized * -(sprintSpeed / 1.5f), ForceMode.Impulse);
                rb.AddForce(moveDirection.normalized * (sprintSpeed / 1.5f), ForceMode.Impulse);
            }
            else
            {
                rb.AddForce(moveDirection.normalized * sprintSpeed, ForceMode.Impulse);
            }
        }
        else
        {
            if (playerLookScript.xRotation < -45f)
            {
                rb.AddForce(transform.up.normalized * (sprintSpeed / (1.5f*groundAirResistRatio)), ForceMode.Impulse);
                rb.AddForce(moveDirection.normalized * (sprintSpeed / (1.5f*groundAirResistRatio)), ForceMode.Impulse);
            }
            else if (playerLookScript.xRotation > 45f)
            {
                rb.AddForce(transform.up.normalized * -(sprintSpeed / (1.5f*groundAirResistRatio)), ForceMode.Impulse);
                rb.AddForce(moveDirection.normalized * (sprintSpeed / (1.5f*groundAirResistRatio)), ForceMode.Impulse);
            }
            else
            {
                rb.AddForce(moveDirection.normalized * (sprintSpeed / groundAirResistRatio), ForceMode.Impulse);
            }
        }

    }
    private void ControlSpeed()
    {
        moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
        
    }

    private void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    private void FixedUpdate()
    {
        if (!wallRunScript.wallLeft && !wallRunScript.wallRight)
        {
            rb.AddForce(Physics.gravity * gravity, ForceMode.Acceleration);
        }
        
        MovePlayer();
    }

    private void MovePlayer()
    {
        
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }

    public void TakeDamage(float damage)
    {
        if (damage >= currentHealth)
        {
            SoundManager.S.MakePlayerDeathSound();
            rb.isKinematic = true;
            StartCoroutine(waitToDie());
        }
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    private IEnumerator waitToDie()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("GameOverScreen");
    }
}
