using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Based on and modified from Plai's youtube tutorial on Rigidbody FPS Movement, https://www.youtube.com/watch?v=LqnPeqoJRFY&list=PLRiqz5jhNfSo-Fjsx3vv2kvYbxUDMBZ0u, https://github.com/Plai-Dev/rigidbody-fps-controller-tutorial
public class WallRun : MonoBehaviour
{
    Animator animator;
    [Header("Movement")]
    public Transform orientation;

    [Header("Detection")]
    public float wallDistance = .5f;
    public float minimumJumpHeight = 1.5f;

    [Header("Wall Running")]
    public float wallRunGravity;
    public float wallRunJumpForce;

    [Header("Camera")]
    public Camera cam;
    public float fov;
    public float wallRunfov;
    public float wallRunfovTime;
    public float camTilt;
    public float camTiltTime;

    public float tilt { get; private set; }

    [HideInInspector] public bool wallLeft = false;
    [HideInInspector] public bool wallRight = false;

    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;

    private Rigidbody rb;

    private bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    private void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance);
    }

    private void Update()
    {
        CheckWall();

        if (CanWallRun())
        {
            if (wallLeft || wallRight)
            {
                StartWallRun();
            }
            else
            {
                StopWallRun();
            }
        }
        else
        {
            StopWallRun();
        }
    }

    private void StartWallRun()
    {
        rb.useGravity = false;

        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunfov, wallRunfovTime * Time.deltaTime);

        if (wallLeft)
        {
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
            animator.SetBool("isWallRunLeft", true);
        }
        else if (wallRight)
        {
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);
            animator.SetBool("isWallRunRight", true);
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallLeft)
            {
                Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
                
            }
            else if (wallRight)
            {
                Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallRunJumpForce * 100, ForceMode.Force);
                
            }
        }
    }

    private void StopWallRun()
    {
        animator.SetBool("isWallRunLeft", false);
        animator.SetBool("isWallRunRight", false);
        rb.useGravity = true;

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunfovTime * Time.deltaTime);
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
    }
}
