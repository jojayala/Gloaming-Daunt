using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Based on and modified from Plai's youtube tutorial on Rigidbody FPS Movement, https://www.youtube.com/watch?v=LqnPeqoJRFY&list=PLRiqz5jhNfSo-Fjsx3vv2kvYbxUDMBZ0u, https://github.com/Plai-Dev/rigidbody-fps-controller-tutorial
public class PlayerLook : MonoBehaviour
{
    [Header("References")]
    public WallRun wallRun;

    public float sensX = 100f;
    public float sensY = 100f;

    public Transform cam = null;
    public Transform orientation = null;

    private float mouseX;
    private float mouseY;

    private float multiplier = 0.01f;

    [HideInInspector] public float xRotation;
    private float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation += mouseX * sensX * multiplier;
        xRotation -= mouseY * sensY * multiplier;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, wallRun.tilt);
        orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
