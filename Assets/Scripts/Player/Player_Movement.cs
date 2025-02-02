using UnityEngine;
using System.Collections;

public class Player_Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private new Collider2D collider;  //this is giving a warning idk why 
    private Collider2D[] results;
    private Vector3 respawnPosition;
    private Vector2 direction;
    private BoxCollider2D boxCollider;


    [Header("Player Settings")]
    public float moveSpeed = 1f;
    public float jumpStrength = 1f;

    [Header("Attack Settings")]
    public float attackDuration = 8f;
    public LayerMask attackableLayers;
    public LayerMask LadderLayer;

    private float horizontalScreenSize = 9.6f;
    private float verticalScreenSize = 5f;
    private bool isGrounded;
    private bool canClimb;
    private bool isClimbing;
    private bool hasWeapon;
    private bool isJumping;
    private bool isAttacking;
    private bool isMoving;

    public GameManager gameManager;

    // Animator code 
    private Animator p_animator;

    AudioManager audioManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        results = new Collider2D[6];
        respawnPosition = transform.position; // Store the initial position
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        boxCollider = GetComponent<BoxCollider2D>(); // Explicitly use BoxCollider2D

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        // animator
        p_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckCollision();
        PlayerMovement();
        // Debug.Log($"Climb: {canClimb} | Attack: {isAttacking} | Grounded: {isGrounded} | Jumping: {isJumping}");
        checkMovment(); // check movment to update bool every time it is moving
        
    }

    private void FixedUpdate()
    {
        rb.velocity = direction;
    }

    private void checkMovment()
    {
        if (Mathf.Abs(rb.velocity.x) != 0)
            p_animator.SetBool("isRunning", true);
        else
            p_animator.SetBool("isRunning", false);


        //Debug.Log($"Velocity X: {rb.velocity.x}, isMoving: {isMoving}");
    }

    private void CheckCollision()
    {
        isGrounded = false;
        canClimb = false;
        hasWeapon = false;
        isClimbing = false;

        Vector2 size = collider.bounds.size;


        Vector2 colliderPosition = (Vector2)transform.position + collider.offset;
        int amount = Physics2D.OverlapBoxNonAlloc(colliderPosition, size, 0f, results);

        for (int i = 0; i < amount; i++)
        {
            GameObject hit = results[i].gameObject;

            if (hit.CompareTag("Ground"))
            {
                isGrounded = results[i].bounds.center.y < (transform.position.y - 0.5f);
                Physics2D.IgnoreCollision(collider, results[i], !isGrounded);
                isJumping = false;

            }
            if (hit.CompareTag("Ladder"))
            {
                canClimb = true;
            }
            if (hit.CompareTag("Weapon"))
            {
                hasWeapon = true;
                StartCoroutine(AttackTime());
                Destroy(hit.gameObject);
            }
            if (hit.CompareTag("Enemy")) // Check if player collides with an enemy
            {
                if (!isAttacking) // Player loses life if not attacking
                {
                    Destroy(hit.gameObject);
                    LoseLife();
                }
            }
        }
    }

    private void PlayerMovement()
    {
        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded && !isClimbing)
        {
            isJumping = true;
            isGrounded = false;
            audioManager.PlaySFX(audioManager.jumping);
            direction = Vector2.up * jumpStrength;
            //Debug.Log($"Climb: {canClimb} | Attack: {isAttacking} | Grounded: {isGrounded} | Jumping: {isJumping}");
            Physics2D.IgnoreLayerCollision(9, 7, true); // Ignore ladder while jumping

        }



        // Climbing
        if (canClimb && !isJumping)
        {
            //If pressing up / down
            if (Input.GetButton("Vertical"))
            {
                isClimbing = true;

            }
            else
            {
                //Debug.Log($"Climb: {canClimb} | Attack: {isAttacking} | Grounded: {isGrounded} | Jumping: {isJumping}");
            }


            if (isClimbing)
            {
                direction.y = Input.GetAxis("Vertical") * moveSpeed;
                Physics2D.IgnoreLayerCollision(9, 7, false); // Allow player to interact with ladder again

            }
        }

        // Walking (Horizontal movement)
        direction.x = Input.GetAxis("Horizontal") * moveSpeed;

        p_animator.SetBool("theClimb", isClimbing);
        p_animator.SetBool("Attk", isAttacking);

        if (isJumping)
        {
            Physics2D.IgnoreLayerCollision(9, 7, true);
            //Debug.Log("WHY DIDNT It ignore it");
        }

        // If the player is on the ground, limit downward velocity
        if (isGrounded)
        {
            isJumping = false;
            direction.y = Mathf.Max(direction.y, -1f);
            Physics2D.IgnoreLayerCollision(9, 7, false); // Re-enable ladder collision after landing
        }

        // Apply gravity when jump execpt while climbing
        if (!isClimbing)
        {
            direction += Physics2D.gravity * Time.deltaTime;
        }

        // Flip player sprite based on movement direction
        if (direction.x != 0)
        {
            transform.eulerAngles = new Vector3(0f, direction.x > 0 ? 0f : 180f, 0f);
        }

        // Store original movement direction
        float originalDirectionX = direction.x;

        // Clamp horizontal position but allow movement back
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -horizontalScreenSize, horizontalScreenSize),
                                         transform.position.y, // No vertical clamping
                                         transform.position.z);

        // Stop movement only if trying to move further out of bounds (horizontal only)
        if (transform.position.x >= horizontalScreenSize && originalDirectionX > 0)
        {
            direction.x = 0; // Stop moving right
        }
        else if (transform.position.x <= -horizontalScreenSize && originalDirectionX < 0)
        {
            direction.x = 0; // Stop moving left
        }

        // If player falls below the screen, trigger death
        if (transform.position.y < -verticalScreenSize)
        {
            Fall();
        }

        // Apply the final calculated velocity
        rb.velocity = direction;

        Physics2D.SyncTransforms(); // Ensure collider updates instantly
    }


    

    private void Attack()
    {
        if (hasWeapon && !isAttacking)
        {
            Debug.Log("Weapon acquired.");
        }
    }

    private IEnumerator AttackTime()
    {
        isAttacking = true;
        float timer = 0f;

        while (timer < attackDuration)
        {
            Collider2D[] attackHits = Physics2D.OverlapCircleAll(transform.position, 1f, attackableLayers);
            foreach (Collider2D hit in attackHits)
            {
                Debug.Log("Attacked: " + hit.name);
                GameObject.Find("GameManager").GetComponent<GameManager>().EarnScore(5);
                Destroy(hit.gameObject);
            }

            timer += 0.5f;
            yield return new WaitForSecondsRealtime(0.5f);
        }

        hasWeapon = false;
        isAttacking = false;
        Debug.Log("Attack duration ended.");
    }



    private void LoseLife()
    {
        transform.position = new Vector3(-7, -4, 0);
        Debug.Log("OH NO");
        audioManager.PlaySFX(audioManager.death);
        GameObject.Find("GameManager").GetComponent<GameManager>().LoseLife(1);
        p_animator.SetBool("nowDead", true);
        StartCoroutine(RespawnAfterDeath());

    }



    private void Fall()
    {
        Debug.Log("Player fell off the screen. Respawning...");
        LoseLife();

        if (gameManager.isPlayerAlive)
        {
            rb.velocity = Vector2.zero; // Stop movement
        }
        else
        {
            Debug.Log("Game Over!");
            Destroy(gameObject);
        }
    }


    private IEnumerator RespawnAfterDeath()
    {
        yield return new WaitForSeconds(2.5f);
        transform.position = respawnPosition;

        Debug.Log("Player Respawned.");
    }
}
