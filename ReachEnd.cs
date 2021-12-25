using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReachEnd : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SoundManager.S.MakeWinSound();
            StartCoroutine(waitToWin());
        }


    }

    private IEnumerator waitToWin()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("WinScreen");
    }
}
