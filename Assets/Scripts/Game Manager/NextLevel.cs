using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{

    public bool level2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger detected with: " + collision.gameObject.name);

        if (collision.gameObject.layer == 9)
        {
            if (level2 == false)
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
