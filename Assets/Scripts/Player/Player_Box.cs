using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Box : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Collider2D[] results;
    private float horizontalInput;
    private float verticalInput;
    //private Vector2 direction;

    [Header("Player Settings")]
    public float moveSpeed = 1f;
    public float jumpStrength = 1f;

    [Header("Attack Settings")]
    public float attackDuration = 8f;
    public LayerMask attackableLayers;
    public LayerMask LadderLayer;

    public int lives = 3;
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
        boxCollider = GetComponent<BoxCollider2D>(); // Explicitly use BoxCollider2D
        results = new Collider2D[6];
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        CheckCollision();
        PlayerMovement();
    }

    private void FixedUpdate()
    {
        // Apply movement only on the x-axis
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y); 
    }

    private void CheckCollision()
    {
        isGrounded = false;
        canClimb = false;
        hasWeapon = false;
        isJumping = false;

        Vector2 size = boxCollider.size;
        Vector2 colliderPosition = (Vector2)transform.position + boxCollider.offset;
        int amount = Physics2D.OverlapBoxNonAlloc(colliderPosition, size, 0f, results);

        for (int i = 0; i < amount; i++)
        {
            GameObject hit = results[i].gameObject;

            // Check for ground collision only if not climbing or jumping
            if (hit.CompareTag("Ground") && !canClimb && !isJumping)
            {
                isGrounded = results[i].bounds.center.y < (transform.position.y - 0.5f);
                Physics2D.IgnoreCollision(boxCollider, results[i], !isGrounded);
            }

            // Prevent ladder interaction while jumping
            if (hit.CompareTag("Ladder") && !isJumping)
            {
                canClimb = true; // Allow climbing only if not jumping
            }

            // Weapon interaction
            if (hit.CompareTag("Weapon"))
            {
                hasWeapon = true;
                StartCoroutine(AttackTime());
                Destroy(hit.gameObject);
            }

            // Enemy interaction
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
        Walk();
        Attack();

        // Flip player sprite based on movement direction
        if (horizontalInput != 0)
            transform.eulerAngles = new Vector3(0f, horizontalInput > 0 ? 0f : 180f, 0f);
    }

    private void Walk()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (!isClimbing)
        {
            verticalInput = rb.velocity.y;
        }
        // Debug.Log($"This is walk. Climb: {canClimb} | Attack: {isAttacking} | Grounded: {isGrounded} | Jumping: {isJumping}");

    }

    private void Jump()
    {
        // Jump only if grounded
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
            isGrounded = false;
            Physics2D.IgnoreCollision(boxCollider, boxCollider, isJumping);
            rb.velocity = new Vector2(rb.velocity.x, jumpStrength); // Apply jump force directly
            Debug.Log($"This is jump. Climb: {canClimb} | Attack: {isAttacking} | Grounded: {isGrounded} | Jumping: {isJumping}");

        }

    }
    private void Climb()
    {
        if (canClimb && !isJumping) 
        {
            verticalInput = Input.GetAxis("Vertical");

            if (Mathf.Abs(verticalInput) > 0) 
            {
                Debug.Log($"This is climb. Climb: {canClimb} | Attack: {isAttacking} | Grounded: {isGrounded} | Jumping: {isJumping}");
                rb.velocity = new Vector2(rb.velocity.x, verticalInput * moveSpeed); 
                rb.gravityScale = 0;
            }
        }
        else
        {
            rb.gravityScale = 1; // Restore gravity when not climbing
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

    public void LoseLife()
    {
        lives--;
        transform.position = new Vector3(-8, -3, 0);

        if (lives == 0)
        {
            Debug.Log("Game Over!");
            Destroy(gameObject);
            gameManager.GameOver();
        }

        GameObject.Find("GameManager").GetComponent<GameManager>().LivesCounter(lives);
    }
}