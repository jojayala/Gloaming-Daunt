using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void btn_StartTheGame()
    {
        SceneManager.LoadScene("GameMap");
    }

    public void btn_QuitGame()
    {
        Debug.Log("QUITTING: The quit command was received");
        Application.Quit();
    }

    public void btn_BackTitle()
    {
        SceneManager.LoadScene("TitleMenu");
    }

    public void btn_Tutorial()
    {
        SceneManager.LoadScene("InstructionMenu");
    }

    public void btn_Credits()
    {
        SceneManager.LoadScene("CreditsMenu");
    }


}
