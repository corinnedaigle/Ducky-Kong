using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Start()
    {
        int savedScore = PlayerPrefs.GetInt("SavedScore", 0);
        UpdateScore(savedScore);
    }

    public void UpdateScore(int newScore)
    {
     scoreText.text = "Score: \n" + newScore;
    }
}