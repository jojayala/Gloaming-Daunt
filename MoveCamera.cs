using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Based on and modified from Plai's youtube tutorial on Rigidbody FPS Movement, https://www.youtube.com/watch?v=LqnPeqoJRFY&list=PLRiqz5jhNfSo-Fjsx3vv2kvYbxUDMBZ0u, https://github.com/Plai-Dev/rigidbody-fps-controller-tutorial
public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition = null;

    void Update()
    {
        transform.position = cameraPosition.position;
    }
}
