using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEditor.SearchService;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject bubbles;
    public GameObject soap;
    public GameObject jumpScore;
    private GameObject toothpaste;
    private GameObject blowdry;

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
        livesText.text = "Lives: " + lives;
        if (lives <= 0)
        {
            GameOver();
            Destroy(player);
        }
    }

    public void GameOver()
    {
        isPlayerAlive = false;
        stop = true;
        gameOverText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(false);
        livesText.gameObject.SetActive(false);

    }

}
