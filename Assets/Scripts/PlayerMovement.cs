using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // === MOVEMENT SETTINGS ===
    public float moveSpeed = 5f;        // How fast player runs forward
    public float laneWidth = 2f;        // Distance between lanes
    public float jumpForce = 6f;        // How high player jumps
    public float laneSwitchSpeed = 10f; // How fast player changes lanes
    
    // === LANE SYSTEM ===
    private int currentLane = 1;        // Start in middle lane (0=left, 1=middle, 2=right)
    private float targetX;              // Where player should move to
    
    // === COMPONENTS ===
    private Rigidbody rb;
    private bool isGrounded = true;     // Can we jump right now?
    
    void Start()
    {
        // Get the Rigidbody component when game starts
        rb = GetComponent<Rigidbody>();
        
        // Calculate starting position
        targetX = (currentLane * laneWidth) - laneWidth;
    }
    
    void Update()
    {
        // === 1. AUTO-RUN FORWARD ===
        // This makes player always move forward
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        
        // === 2. LANE SWITCHING ===
        // Press A to move left
        if (Input.GetKeyDown(KeyCode.A) && currentLane > 0)
        {
            currentLane--;  // Change to left lane
            Debug.Log("Moving LEFT to lane: " + currentLane);
        }
        
        // Press D to move right
        if (Input.GetKeyDown(KeyCode.D) && currentLane < 2)
        {
            currentLane++;  // Change to right lane
            Debug.Log("Moving RIGHT to lane: " + currentLane);
        }
        
        // Calculate where player should be
        targetX = (currentLane * laneWidth) - laneWidth;
        
        // Smoothly move player to target lane
        Vector3 targetPosition = new Vector3(
            targetX,
            transform.position.y,  // Keep same height
            transform.position.z   // Keep same forward position
        );
        
        // Lerp = smooth movement (not instant teleport)
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            Time.deltaTime * laneSwitchSpeed
        );
        
        // === 3. JUMPING ===
        // Press Space to jump (only if on ground)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isGrounded = false;  // Can't jump again until we land
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("JUMP!");
        }
    }
    
    // Detect when player touches ground
    void OnCollisionEnter(Collision collision)
    {
        // If we hit something below us, we're grounded
        if (collision.gameObject.CompareTag("Ground") || 
            collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
        }
    }
}