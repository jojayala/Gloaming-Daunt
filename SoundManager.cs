using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager S; // Singleton Definition

    private AudioSource audio;
    public AudioClip airSwingSound;
    
    public AudioClip shurikenClashSound;
    public AudioSource ambientSound;
    public AudioClip enemyDeathClip;
    public AudioClip playerDeathClip;
    public AudioClip playerHitClip;
    public AudioClip throwSound;
    public AudioClip dashSound;
    public AudioClip playerJumpClip;
    public AudioClip selectSound;
    
    public AudioClip winSound;

    private void Awake()
    {
        S = this; // singleton is assigned
    }
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void MakeAirSwingSound()
    {
        audio.PlayOneShot(airSwingSound, 10.0f);
    }
    
    public void MakeShurikenClashSound()
    {
        audio.PlayOneShot(shurikenClashSound, 0.5f);
    }

    public void MakeThrowSound()
    {
        audio.PlayOneShot(throwSound);
    }
    public void MakeSelectSound()
    {
        audio.PlayOneShot(selectSound);
    }
    public void MakeDashSound()
    {
        audio.PlayOneShot(dashSound);
    }
    public void MakeEnemyDeathSound()
    {

        audio.PlayOneShot(enemyDeathClip, 1.0f);
    }

    public void MakePlayerDeathSound()
    {

        audio.PlayOneShot(playerDeathClip, 2.0f);
    }

    public void PlayAmbientSound()
    {
        ambientSound.Play();
    }

    public void StopAllSounds()
    {
        // stop the ambient noise
        ambientSound.Stop();
    }

    public void MakePlayerHitSound()
    {

        audio.PlayOneShot(playerHitClip, 1.0f);
    }


    public void MakePlayerJumpSound()
    {

        audio.PlayOneShot(playerJumpClip, 2.0f);
    }

   

    public void MakeWinSound()
    {
        audio.PlayOneShot(winSound, 1.0f);
    }
}
