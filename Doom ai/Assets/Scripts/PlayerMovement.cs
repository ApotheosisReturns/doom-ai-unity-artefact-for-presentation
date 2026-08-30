using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 7f;        // Constant movement speed (DOOM-style)
    public float gravity = -20f;        // Downward force applied every frame
    public float jumpHeight = 1.5f;     // Jump strength

    [Header("Mouse Look")]
    public float mouseSensitivity = 2f; // How fast the camera rotates
    public Transform cameraTransform;   // Reference to the player's camera

    private CharacterController controller; // Unity's built-in movement component
    private Vector3 velocity;               // Vertical velocity (gravity + jumping)
    private float xRotation = 0f;           // Vertical camera rotation accumulator

    private void Start()
    {
        // Cache the CharacterController component
        controller = GetComponent<CharacterController>();

        // Lock the mouse cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleMovement();   // WASD movement + gravity + jumping
        HandleMouseLook();  // Camera rotation
    }

    private void HandleMovement()
    {
        // --- Horizontal Movement (WASD) ---
        // Raw input gives instant DOOM-style movement (no smoothing)
        float x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        float z = Input.GetAxisRaw("Vertical");   // W/S or Up/Down

        // Convert input into world-space movement
        Vector3 move = transform.right * x + transform.forward * z;

        // Apply movement using CharacterController
        controller.Move(move * moveSpeed * Time.deltaTime);

        // --- Gravity ---
        // If grounded, reset downward velocity
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f; // Small negative keeps player "stuck" to ground

        // Apply gravity over time
        velocity.y += gravity * Time.deltaTime;

        // --- Jumping ---
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            // Jump formula: v = sqrt(height * -2 * gravity)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply vertical movement (gravity + jump)
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleMouseLook()
    {
        // --- Mouse Input ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity; // Horizontal look
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity; // Vertical look

        // --- Horizontal Rotation ---
        // Rotate the player body left/right
        transform.Rotate(Vector3.up * mouseX);

        // --- Vertical Rotation ---
        // Accumulate vertical rotation
        xRotation -= mouseY;

        // Clamp vertical rotation so player can't flip upside-down
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        // Apply vertical rotation to camera only
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
