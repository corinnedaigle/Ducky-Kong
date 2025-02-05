using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public bool level2;
    private AudioManager audioManager;

    private void Start()
    {
        // Get the AudioManager instance
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger detected with: " + collision.gameObject.name);

        if (collision.gameObject.layer == 9) // Check if it's the player
        {
            if (audioManager != null)
            {
                audioManager.PauseBGM(); // Stop the background music before loading the next level
            }

            if (!level2)
            {
                SceneManager.LoadScene("Level 2");
            }
            else
            {
                SceneManager.LoadScene("Win");
            }
        }
    }
}
