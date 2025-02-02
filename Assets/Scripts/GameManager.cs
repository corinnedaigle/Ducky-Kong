using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject bubbles;
    public GameObject soap;
    public GameObject jumpScore;
    private GameObject toothpaste;
    private GameObject blowdry;

    // place heart object in here
    public GameObject live1;
    public GameObject live2;
    public GameObject live3;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI gameOverText;

    public bool isPlayerAlive;

    private int score;
    private int lives;

    public GameObject[] enemies;

    public Vector3 spawnValues;
    public float spawnWait;
    public float spawnMostWait;
    public float spawnLeastWait;
    public int startWait;
    public bool stop;

    private int randomEnemy;

    // Start is called before the first frame update
    void Start()
    {
        gameOverText.gameObject.SetActive(false);
        StartCoroutine(waitSpawner());
        score = 0;
        lives = 3;
        scoreText.text = "Score: " + score;
        livesText.text = "Lives: " + lives;
        isPlayerAlive = true;
        stop = false;
    }

    // Update is called once per frame
    void Update()
    {
        spawnWait = Random.Range(spawnLeastWait, spawnMostWait); 
    }

    IEnumerator waitSpawner()
    {
        yield return new WaitForSeconds(startWait);

        while (!stop)
        {
            randomEnemy = Random.Range(0, 2);
            Vector3 spawnPosition = new Vector3(1, 1, 0);
            Instantiate(enemies[randomEnemy], spawnPosition + transform.TransformPoint(0, 0, 0), gameObject.transform.rotation);
            yield return new WaitForSeconds(spawnWait);
        }
    }

    // Score keeper
    public void EarnScore(int newScore)
    {
        scoreText.text = "Score: " + newScore;
    }
    // Updates lives
    public void LoseLife(int HowMuchItLose)
    {
        lives-= HowMuchItLose;

        switch (lives)
        {
            // each one of this distroy a heart icon from the player UI 
            case 2:
                    Destroy(live1);
                    break;
            case 1: 
                Destroy(live2);
                break;
            case 0:
                Destroy(live3);
                GameOver();
                Destroy(player);
                break;

        }
   // Your previous code in case i brake something by accident

        /*livesText.text = "Lives: " + lives;
        if (lives <= 0)
        {
            GameOver();
            Destroy(player);
        }*/
    }

    public void GameOver()
    {
        isPlayerAlive = false;
        stop = true;
        gameOverText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(false);
        livesText.gameObject.SetActive(false);

        // I added this so it sends you to the game over screen instead 
        SceneManager.LoadScene("Game Over");

    }

}
