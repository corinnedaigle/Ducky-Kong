using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static Cinemachine.DocumentationSortingAttribute;

public class JumpScore : MonoBehaviour
{
    public Transform player;
    bool IsPlayerInRange;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == player)
        {
            IsPlayerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform == player)
        {
            IsPlayerInRange = false;
        }
    }
    void Update()
    {
        if (IsPlayerInRange == true)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().EarnScore(100);
        }
    }
}
