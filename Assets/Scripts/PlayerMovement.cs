using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 6f;
    public float laneDistance = 2f;
    public float jumpForce = 10f;
    public static bool isGameOver = false;
    
    [Header("UI Reference")]
    public GameUI gameUI; // Drag UIManager here

    private Rigidbody rb;
    private int currentLane = 0;
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        // Move Left
        if (Input.GetKeyDown(KeyCode.A) && currentLane > -1)
            currentLane--;

        // Move Right
        if (Input.GetKeyDown(KeyCode.D) && currentLane < 1)
            currentLane++;

        // Smooth lane transition
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
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Land on ground
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
            
        // Land on yellow obstacle (bounce platform)
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
    
    // ========== ADDED FUNCTIONS ==========
    
    // Collect box and add score
    void CollectBox(GameObject box)
    {
        Debug.Log("Box collected!");
        
        // Add score in UI
        if (gameUI != null)
        {
            gameUI.CollectBox(2); // Add 2 points
        }
        
        // Hide the box
        box.SetActive(false);
        
        // Or destroy it
        // Destroy(box);
    }
    
    // Bounce higher on yellow obstacle
    void BounceOnYellowObstacle()
    {
        Debug.Log("Bounced on yellow obstacle!");
        
        // Higher jump
        rb.AddForce(Vector3.up * (jumpForce * 1.5f), ForceMode.Impulse);
        
        // You can add effects here
    }
    
    // Check if player is trapped by boxes
    void CheckIfTrapped()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
        int boxCount = 0;
        
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Box") || col.CompareTag("Obstacle"))
                boxCount++;
        }
        
        // If surrounded from 4 sides
        if (boxCount >= 4)
        {
            PlayerTrapped();
        }
    }
    
    // When player gets trapped
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
        
        // Show Game Over screen in UI
        if (gameUI != null)
        {
            gameUI.ShowGameOver();
        }
        
        // Stop time
        Time.timeScale = 0f;
    }
    
    // Test Game Over function
    void TestGameOver()
    {
        if (!isGameOver)
        {
            Debug.Log("Testing Game Over");
            TriggerGameOver();
        }
    }
    
    // Draw detection sphere in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}