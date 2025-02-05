using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        // esc to end game 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            Debug.Log("quit aplication");
        }
    }
}
