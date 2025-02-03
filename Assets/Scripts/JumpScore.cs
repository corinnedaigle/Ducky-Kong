using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static Cinemachine.DocumentationSortingAttribute;

public class JumpScore : MonoBehaviour
{

    bool IsPlayerInRange;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 9)
        {
            Debug.Log("Take points");
            IsPlayerInRange = true;
            GameObject.Find("GameManager").GetComponent<GameManager>().EarnScore(5);

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 9)
        {
            IsPlayerInRange = false;
        }
    }
}