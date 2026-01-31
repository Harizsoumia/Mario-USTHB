using UnityEngine;

public class MarioRunnerController : MonoBehaviour
{
    public float forwardSpeed = 8f;
    public float sideSpeed = 6f;
    public float jumpForce = 8f;
    public float gravity = -20f;

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");

        // Forward + side movement
        Vector3 move = transform.forward * forwardSpeed + transform.right * h * sideSpeed;

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            velocity.y = jumpForce;
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;

        // Apply movement
        controller.Move((move + velocity) * Time.deltaTime);
    }
}
