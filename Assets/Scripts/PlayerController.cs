using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 10f;
    public float sideSpeed = 5f;
    public float maxSidePosition = 3f; // How far left/right player can go
    
    [Header("Jump Settings")]
    public float jumpForce = 10f;
    public float gravity = -20f;
    
    [Header("Speed Increase")]
    public float speedIncreaseRate = 0.1f; // Speed increase per second
    public float maxSpeed = 25f;

    private CharacterController controller;
    private Vector3 moveDirection;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("CharacterController component missing! Please add one to the player.");
        }
    }

    void Update()
    {
        // Check if grounded
        isGrounded = controller.isGrounded;

        // Forward movement (constant)
        moveDirection.z = forwardSpeed;

        // Increase speed over time
        if (forwardSpeed < maxSpeed)
        {
            forwardSpeed += speedIncreaseRate * Time.deltaTime;
        }

        // Side movement (A/D or Left/Right arrows)
        float horizontalInput = Input.GetAxis("Horizontal");
        moveDirection.x = horizontalInput * sideSpeed;

        // Clamp side position
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -maxSidePosition, maxSidePosition);
        transform.position = clampedPosition;

        // Jumping
        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveDirection.y = jumpForce;
            }
            else
            {
                moveDirection.y = 0f;
            }
        }
        else
        {
            // Apply gravity
            moveDirection.y += gravity * Time.deltaTime;
        }

        // Move the player
        controller.Move(moveDirection * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Handle collision with obstacles
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Collect coins
        if (other.CompareTag("Coin"))
        {
            GameManager.Instance.AddScore(1);
            Destroy(other.gameObject);
        }
    }
}