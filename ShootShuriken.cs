using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootShuriken : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject shuriken;
    public float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Quaternion rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x, this.transform.eulerAngles.y - 90, this.transform.eulerAngles.z);
        
        GameObject cS = Instantiate(shuriken, spawnPoint.position, rotation);
        Rigidbody rig = cS.GetComponent<Rigidbody>();
        rig.AddForce(spawnPoint.forward * speed, ForceMode.Impulse);
        rig.AddForce(spawnPoint.up * (speed / 10), ForceMode.Impulse);
    }
}
