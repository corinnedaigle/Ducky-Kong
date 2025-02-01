using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
 
    public bool level2;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && level2 != true)
        {
            SceneManager.LoadScene("level2");
        }
        else 
        {
            SceneManager.LoadScene("win");
        }
    }
}
