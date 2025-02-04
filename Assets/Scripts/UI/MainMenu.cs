using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1f;
        //score = PlayerPrefs.GetInt("SavedScore", 0); // Load saved score
        ResetGameData(); // Reset score and lives when opening the main menu

    }

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

    // Load Main Menu
    public void LoadMainMenu()
    {
        ResetGameData(); // Reset everything when returning to the main menu
        SceneManager.LoadScene("Main Menu");
    }

    // Reset game data (score and lives)
    private void ResetGameData()
    {
        PlayerPrefs.SetInt("SavedScore", 0);
        PlayerPrefs.Save();
    }

}
