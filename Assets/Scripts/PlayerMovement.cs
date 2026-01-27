using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float forwardSpeed = 6f;
    public float laneDistance = 2f;
    public float jumpForce = 10f;
    public static bool isGameOver = false;

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
        Jump();
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

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }
    
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log(" Game Over!");
            isGameOver = true;
            Time.timeScale = 0f;
        }
    }
}