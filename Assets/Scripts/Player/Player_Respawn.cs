using UnityEngine;
using System.Collections;

public class Player_Respawn : MonoBehaviour
{
    private Vector3 respawnPosition;
    private bool takenWeapon;
    private GameManager gameManager;
    private AudioManager audioManager;
    private Animator p_animator;
    public GameObject weapon;
    private Vector3 weaponSpawn;

    private void Awake()
    {
        respawnPosition = transform.position;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        p_animator = GetComponent<Animator>();
        weaponSpawn = weapon.transform.position;
        takenWeapon = false;
    }

    public void Fall()
    {
        Debug.Log("Player fell off the screen. Respawning...");
        ResultOfLosingLife();

        if (gameManager.isPlayerAlive)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        else
        {
            Debug.Log("Game Over!");
            Destroy(gameObject);
        }
    }

    public void ResultOfLosingLife()
    {
        Debug.Log("OH NO");
        audioManager.PlaySFX(audioManager.death);
        gameManager.LoseLife(1);
        p_animator.SetBool("nowDead", true);
        p_animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        Time.timeScale = 0;
        StartCoroutine(RespawnAfterDeath());
    }

    private IEnumerator RespawnAfterDeath()
    {
        yield return new WaitForSecondsRealtime(1.5f);

        transform.position = respawnPosition;

        p_animator.Rebind();
        p_animator.Update(0);
        p_animator.SetBool("nowDead", false);
        p_animator.updateMode = AnimatorUpdateMode.Normal;

        if (takenWeapon)
            Instantiate(weapon, weaponSpawn, Quaternion.identity);

        Time.timeScale = 1;
    }
}
