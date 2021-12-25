using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientPlayer : MonoBehaviour
{
    public Transform cameraOrientation = null;

    void Update()
    {
        transform.rotation = cameraOrientation.rotation;
    }
}
