using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void LaunchMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ClearImageCache()
    {
        Davinci.ClearAllCachedFiles();
    }

    public void LaunchDbD()
    {
        SceneManager.LoadScene(1);
    }
    public void LaunchOW()
    {
        SceneManager.LoadScene(2);
    }
    public void ExitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
