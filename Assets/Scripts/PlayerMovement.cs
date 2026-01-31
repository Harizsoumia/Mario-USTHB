using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 6f;
    public float laneDistance = 2f;
    public float jumpForce = 10f;
    public static bool isGameOver = false;

    [Header("UI Reference")]
    public GameUI gameUI;

    [Header("Audio")]
    public AudioClip jumpSound;
    private AudioSource audioSource;

    private Rigidbody rb;
    private int currentLane = 0;
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        isGameOver = false;
    }

    void Update()
    {
        if (isGameOver) return;

        MoveForward();
        HandleLaneSwitch();
        HandleJump();

        // Test Game Over with T key
        if (Input.GetKeyDown(KeyCode.T))
        {
            TestGameOver();
        }
    }

    void FixedUpdate()
    {
        if (!isGameOver)
        {
            // Check if trapped every 30 frames (for performance)
            if (Time.frameCount % 30 == 0)
            {
                CheckIfTrapped();
            }
        }
    }

    void MoveForward()
    {
        Vector3 v = rb.linearVelocity;
        v.z = forwardSpeed;
        rb.linearVelocity = v;
    }

    void HandleLaneSwitch()
    {
        if (Input.GetKeyDown(KeyCode.A) && currentLane > -1)
            currentLane--;

        if (Input.GetKeyDown(KeyCode.D) && currentLane < 1)
            currentLane++;

        Vector3 targetPosition = transform.position;
        targetPosition.x = currentLane * laneDistance;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;

            // Play jump sound
            if (jumpSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(jumpSound);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;

        if (collision.gameObject.CompareTag("YellowObstacle"))
        {
            isGrounded = true;
            BounceOnYellowObstacle();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Hit obstacle = Game Over
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over - Hit obstacle!");
            TriggerGameOver();
        }

        // Collect box = Add score
        if (other.CompareTag("Box"))
        {
            CollectBox(other.gameObject);
        }
    }

    // Collect box and add score
    void CollectBox(GameObject box)
    {
        Debug.Log("Box collected!");

        if (gameUI != null)
        {
            gameUI.CollectBox(2);
        }

        box.SetActive(false);
    }

    // Bounce on yellow obstacle
    void BounceOnYellowObstacle()
    {
        Debug.Log("Bounced on yellow obstacle!");
        rb.AddForce(Vector3.up * (jumpForce * 1.5f), ForceMode.Impulse);
    }

    // Check if player is trapped
    void CheckIfTrapped()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
        int boxCount = 0;

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Box") || col.CompareTag("Obstacle"))
                boxCount++;
        }

        if (boxCount >= 4)
        {
            PlayerTrapped();
        }
    }

    // Player trapped
    void PlayerTrapped()
    {
        if (isGameOver) return;

        Debug.Log("Player trapped in box!");
        TriggerGameOver();
    }

    // Trigger Game Over
    void TriggerGameOver()
    {
        isGameOver = true;

        if (gameUI != null)
        {
            gameUI.ShowGameOver();
        }

        Time.timeScale = 0f;
    }

    // Test Game Over
    void TestGameOver()
    {
        if (!isGameOver)
        {
            Debug.Log("Testing Game Over");
            TriggerGameOver();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}