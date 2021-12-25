using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHit : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        GameObject enemy;
        if (collision.gameObject.tag == "Enemy")
        {
            enemy = collision.gameObject;
            SoundManager.S.MakeEnemyDeathSound();           
            EnemyScript enemyScript = enemy.GetComponentInParent<EnemyScript>();
            enemyScript.isDead = true;
          
           
        }
        else if (collision.gameObject.tag == "Shuriken")
        {
            SoundManager.S.MakeShurikenClashSound();
            Destroy(collision.gameObject);
        }

    }
}
