using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startMenu : MonoBehaviour
{
    public void loadMain()
    {
        SceneManager.LoadScene("Main");
    }
    public void quitgame()
    {
        Application.Quit();
    }
}
