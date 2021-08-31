using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class settingPage : MonoBehaviour
{
    public InputField cheatField;
    public generator gameManager;
    public fpsPlayer myPlayer;

    public Slider volunm;
    public Slider sensityvity;

    void Start()
    {
        volunm.value = gameManager.volunm;
        sensityvity.value = myPlayer.mouseSensityvity;
    }

    void Update()
    {
        gameManager.volunm = volunm.value;
        myPlayer.mouseSensityvity = sensityvity.value;
    }

    public void cheatGo()
    {
        int cheatCode = int.Parse(cheatField.text);
        gameManager.addScore(cheatCode);
    }

    public void restartGo()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void mainmenuGo()
    {
        SceneManager.LoadScene("Start");
    }

    public void settingEnd()
    {
        gameManager.endSetting();
    }
}
