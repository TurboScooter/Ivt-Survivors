using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransferButtons : MonoBehaviour
{
    public void ShopButton()
    {
        SceneManager.LoadScene(4);
    }

    public void HomeScreenButton()
    {
        SceneManager.LoadScene(0);
    }

    public void GameOverButton()
    {
        SceneManager.LoadScene(2);
    }

    public void CreditsButton()
    {
        SceneManager.LoadScene(3);
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void GameQuitButton() 
    {
        Application.Quit();
    }
}
