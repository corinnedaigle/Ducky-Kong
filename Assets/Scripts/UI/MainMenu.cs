using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }
    public void LoadCredit()
    {
        SceneManager.LoadScene("Credits");
    }
    public void LoadQuit()
    {
        Application.Quit();
    }

}
