using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject bubbles;
    public GameObject soap;

    // place heart object in here
    public GameObject live1;
    public GameObject live2;
    public GameObject live3;

    public TextMeshProUGUI scoreText;

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

        StartCoroutine(waitSpawner());
        score = 0;
        lives = 3;
        scoreText.text = "" + score;
        isPlayerAlive = true;
        stop = false;
        score = PlayerPrefs.GetInt("SavedScore", 0); // Load saved score
        

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
            randomEnemy = Random.Range(0, 4);
            Vector3 spawnPosition = new Vector3(1, 1, 0);
            Instantiate(enemies[randomEnemy], spawnPosition + transform.TransformPoint(0, 0, 0), gameObject.transform.rotation);
            yield return new WaitForSeconds(spawnWait);
        }
    }

    // Score keeper
    public void EarnScore(int HowMuchItEarn)
    {
        score += HowMuchItEarn;
        PlayerPrefs.SetInt("SavedScore", score); // Save the score
        PlayerPrefs.Save(); // Make sure it persists
        scoreText.text = "Score: " + score;
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

        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            // Clear existing enemies
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                Destroy(enemy);
            }


            // Restart enemy spawning if the player is still alive
            if (lives > 0)
            {
                StopCoroutine(waitSpawner()); // Stop the old coroutine
                StartCoroutine(waitSpawner()); // Start it again
            }
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
        scoreText.gameObject.SetActive(false);

        PlayerPrefs.SetInt("SavedScore", 0);
        PlayerPrefs.Save();

        // I added this so it sends you to the game over screen instead 
        SceneManager.LoadScene("Game Over");
    }

}
