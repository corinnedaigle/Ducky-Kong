using UnityEngine;
using System.Collections;


public class Player_Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private new Collider2D collider;


    // Array to store results of collision checks
    private Collider2D[] results;
    private Vector2 direction;

    public float moveSpeed = 1f;
    public float jumpStrength = 1f;

    // Booleans to check the player's state
    private bool isGrounded;
    private bool isClimbing;
    private bool hasWeapon;

    public float attackDuration = 8f;
    public LayerMask attackableLayers;


    private bool isAttacking; // Prevent multiple attacks

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        // Set max collisions to 4
        results = new Collider2D[4];
    }

    private void CheckCollision()
    {
        isGrounded = false;
        isClimbing = false;
        hasWeapon = false;

        // Adjust the size of the collision box slightly
        Vector2 size = collider.bounds.size;
        size.y += 0.1f; // Small increase to detect grounded state
        size.x /= 2f;   // Small decrease to narrow the box

        Vector2 colliderPosition = (Vector2)transform.position + collider.offset;
        int amount = Physics2D.OverlapBoxNonAlloc(colliderPosition, size, 0f, results);

        for (int i = 0; i < amount; i++)
        {
            GameObject hit = results[i].gameObject;

            // Check if it's a ground object (tiles or other platforms)
            if (hit.CompareTag("Ground"))
            {
                // Only set as grounded if the platform is below the player
                isGrounded = results[i].bounds.center.y < (transform.position.y);

                // Ignore collisions with platforms not below the player
                Physics2D.IgnoreCollision(collider, results[i], !isGrounded);
            }
            // Check if it's a ladder
            else if (hit.CompareTag("Ladder"))
            {
                isClimbing = true;
            }
            // Check if the player picks up a weapon
            else if (hit.CompareTag("Weapon"))
            {
                hasWeapon = true;
                StartCoroutine(Attack());
                Destroy(hit.gameObject); // Pick up the weapon
            }
        }
    }

    private void Update()
    {
        // Check for collisions every frame
        CheckCollision();

        Movement();

        // Horizontal movement
        direction.x = Input.GetAxis("Horizontal") * moveSpeed;

        // Prevent downward movement from exceeding the threshold
        if (isGrounded)
        {
            direction.y = Mathf.Max(direction.y, -1f);
        }

        // Flip the player sprite based on movement direction
        if (direction.x > 0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (direction.x < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = direction;
    }

    private void Movement()
    {
        // Allow vertical movement when climbing a ladder
        if (isClimbing)
        {
            direction.y = Input.GetAxis("Vertical") * moveSpeed;
        }
        // Allow jumping only when grounded
        else if (Input.GetButtonDown("Jump") && isGrounded)
        {
            direction.y = jumpStrength;
        }
        // Allow attack only when has weapon and not already attacking
        else if (hasWeapon && !isAttacking)
        {
            Debug.Log("Weapon Status:" + hasWeapon);
        }
        else
        {
            // Apply gravity when not climbing or jumping
            direction.y += Physics2D.gravity.y * Time.deltaTime;
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        float timer = 0f;

        while (timer < attackDuration)
        {
            // Check for attackable objects within range
            Collider2D[] attackHits = Physics2D.OverlapCircleAll(transform.position, 1f, attackableLayers);

            foreach (Collider2D hit in attackHits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    Debug.Log("Attacked: " + hit.name);
                    Destroy(hit.gameObject);
                }
            }

            timer += 0.5f; // Attack every 0.5 seconds
            yield return new WaitForSeconds(0.5f);
        }

        hasWeapon = false; // Reset weapon state after the attack duration
        isAttacking = false; // End attacking state
        Debug.Log("Attack duration ended.");
    }
}