using UnityEngine;
using System.Collections;

public class Player_Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private new Collider2D collider;
    private Collider2D[] results;
    private Vector3 respawnPosition;
    private Vector2 direction;

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

    public GameManager gameManager;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        results = new Collider2D[6];
        respawnPosition = transform.position; // Store the initial position
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();


    }

    private void Update()
    {
        CheckCollision();
        PlayerMovement();
        // Debug.Log($"Climb: {canClimb} | Attack: {isAttacking} | Grounded: {isGrounded} | Jumping: {isJumping}");
    }

    private void FixedUpdate()
    {
        rb.velocity = direction;
    }

    private void CheckCollision()
    {
        isGrounded = false;
        canClimb = false;
        hasWeapon = false;

        Vector2 size = collider.bounds.size;


        Vector2 colliderPosition = (Vector2)transform.position + collider.offset;
        int amount = Physics2D.OverlapBoxNonAlloc(colliderPosition, size, 0f, results);

        for (int i = 0; i < amount; i++)
        {
            GameObject hit = results[i].gameObject;

            if (hit.CompareTag("Ground"))
            {
                bool wasJumping = isJumping; // Store previous state
                isGrounded = results[i].bounds.center.y < (transform.position.y - 0.5f);

                if (isGrounded && wasJumping)
                {
                    isJumping = false; // Reset jump when landing
                }

                Physics2D.IgnoreCollision(collider, results[i], !isGrounded);
            }
            if (hit.CompareTag("Ladder") && !isJumping)
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
        Jump();
        Climb();
        Attack();
        Walk();

        // Flip player sprite based on movement direction
        if (direction.x != 0) transform.eulerAngles = new Vector3(0f, direction.x > 0 ? 0f : 180f, 0f);

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

        Physics2D.SyncTransforms(); // Ensure collider updates instantly

    }

    private void Walk()
    {
        direction.x = Input.GetAxis("Horizontal") * moveSpeed;
        direction.y += Physics2D.gravity.y * Time.deltaTime;

        if (isGrounded) direction.y = Mathf.Max(direction.y, -1f); 
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Debug.Log("JUMP");
            isJumping = true;
            isGrounded = false;
            direction.y = jumpStrength;
        }
    }

    private void Climb()
    {
        if (canClimb && !isJumping)
        {
            if (Input.GetButtonDown("Vertical"))
            {
                isClimbing = true;
            }


            if (isClimbing)
            {
                direction.y = Input.GetAxis("Vertical") * moveSpeed;
            }
        }
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

        Debug.Log("OH NO");
        GameObject.Find("GameManager").GetComponent<GameManager>().LoseLife(1);
    }

    private void Fall()
    {
        Debug.Log("Player fell off the screen. Respawning...");
        LoseLife();

        if (gameManager.isPlayerAlive)
        {
            transform.position = respawnPosition; // Reset position
            rb.velocity = Vector2.zero; // Stop movement
        }
        else
        {
            Debug.Log("Game Over!");
            Destroy(gameObject);
        }
    }


}
