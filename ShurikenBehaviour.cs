using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenBehaviour : MonoBehaviour
{
    public int damage = 10;
    private void OnCollisionEnter(Collision collision)
    {
        GameObject player;
        if (collision.gameObject.tag == "Player")
        {
            SoundManager.S.MakePlayerHitSound();


            player = collision.gameObject;
            
            PlayerMovement playerScript = player.GetComponentInParent<PlayerMovement>();
            playerScript.TakeDamage(damage);



        }
        
        Destroy(gameObject);

    }
}
