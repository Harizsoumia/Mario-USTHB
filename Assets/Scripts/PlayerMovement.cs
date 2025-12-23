using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float forwardSpeed = 6f;
    public float laneDistance = 2f;
    public float jumpForce = 7f;

    private Rigidbody rb;
    private int currentLane = 0; // -1 left, 0 middle, 1 right
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        MoveForward();
        HandleLaneSwitch();
        Jump();
    }

    void MoveForward()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, forwardSpeed);
    }

    void HandleLaneSwitch()
    {
        if (Input.GetKeyDown(KeyCode.A))
            currentLane--;

        if (Input.GetKeyDown(KeyCode.D))
            currentLane++;

        currentLane = Mathf.Clamp(currentLane, -1, 1);

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
}