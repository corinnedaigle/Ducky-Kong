using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private new Collider2D collider;

    // Array to store results of collision checks
    private Collider2D[] results;
    private Vector2 direction;

    public float moveSpeed = 1f;
    public float jumpStrengh = 1f;

    // Booleans to check the player's state
    private bool isGrounded;
    private bool isClimbing;
    private bool hasWeapon;

    // public float attackDuration = 5f;
    // public LayerMask attackableLayers;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        // Set max collisions to 4
        results = new Collider2D[4];
    }

    // Check for collisions with the ground or ladder
    private void CheckCollision()
    {
        isGrounded = false;
        isClimbing = false;
        hasWeapon = false;

        // Adjust the size of the collision box slightly
        Vector2 size = collider.bounds.size;
        size.y += 0.1f;
        size.x /= 2f;

        // Check for overlaps with objects in the specified area
        int amount = Physics2D.OverlapBoxNonAlloc(transform.position, size, 0f, results);

        for (int i = 0; i < amount; i++)
        {
            GameObject hit = results[i].gameObject;

            // Check if it's a ground
            if (hit.layer == LayerMask.NameToLayer("Ground"))
            {
                // Only set as grounded if the platform is below the player
                isGrounded = hit.transform.position.y < (transform.position.y - 0.5f);

                // Ignore collisions with platforms not below the player
                Physics2D.IgnoreCollision(collider, results[i], !isGrounded);
            }
            // Check if it's a ladder
            else if (hit.layer == LayerMask.NameToLayer("Ladder"))
            {
                isClimbing = true;
                // Debug.Log(isClimbing);
            }
            else if (hit.layer == LayerMask.NameToLayer("Weapon"))
            {
                hasWeapon = true;
                // Debug.Log(hasWeapon);
            }

        }
    }

    private void Update()
    {
        // Check for collisions every frame
        CheckCollision();
        // Debug.Log(isGrounded);

        // Allow vertical movement when climbing a ladder
        if (isClimbing)
        {
            direction.y = Input.GetAxis("Vertical") * moveSpeed;
        }
        // Allow jumping only when grounded
        else if (Input.GetButtonDown("Jump") && isGrounded)
        {
            direction = Vector2.up * jumpStrengh;
        }

        //Allow attack only when has weapon
        /*else if (hasWeapon)
        {

        }*/

        // Apply gravity when not climbing or jumping
        else
        {
            direction += Physics2D.gravity * Time.deltaTime;
        }

        direction.x = Input.GetAxis("Horizontal") * moveSpeed;

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



    //attack logic?????

    /*    IEnumerator Attack()
        {
            float timer = 0f;

            while (timer < attackDuration)
            {
                // Check for attackable objects within range
                Collider2D[] attackHits = Physics2D.OverlapCircleAll(transform.position, 1f, attackableLayers);

                foreach (Collider2D hit in attackHits)
                {
                    // Debug.Log("Attacked: " + hit.name);
                }

                timer += 0.5f; // Attack every 0.5 seconds
                yield return new WaitForSeconds(0.5f);
            }

            hasWeapon = false; // Reset weapon state after the attack duration
            Debug.Log("Attack duration ended.");
        }

    */
}