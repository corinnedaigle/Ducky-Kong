using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Send player to Level 1
    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }
    // Send to Credit scne 
    public void LoadCredit()
    {
        SceneManager.LoadScene("Credits");
    }

    // Quit the game  
    public void LoadQuit()
    {
        Application.Quit();
    }
    
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

}
