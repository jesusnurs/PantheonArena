using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float acceleration = 8f;
    [SerializeField] private float deceleration = 12f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    
    [Header("Gravity Settings")]
    [SerializeField] private float fallMultiplier = 2.5f;
    
    private bool isGrounded;
    private Vector3 moveDirection;
    private Vector3 currentVelocity;
    private Animator animator;
    private Rigidbody rb;
    private void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, groundCheckDistance, groundLayer);
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        CheckGrounded();
        // Get input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Convert input to isometric direction
        Vector3 input = new Vector3(horizontal, 0f, vertical).normalized;
    
        // Convert to isometric space
        moveDirection = ConvertToIsometric(input);
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Debug.Log("Jump button pressed");
            Debug.Log("Is Grounded: " + isGrounded);
            Jump();
        }
    }
    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    
        // Optional: Trigger jump animation
        if (animator != null)
        {
            animator.SetTrigger("Jump");
        }
    }
    private void FixedUpdate()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        // Apply movement
        if (moveDirection.magnitude > 0.1f)
        {
            // Accelerate
            currentVelocity = Vector3.Lerp(currentVelocity, moveDirection * moveSpeed, acceleration * Time.fixedDeltaTime);
        
            // Rotate character to face movement direction
            if (currentVelocity.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            }
        }
        else
        {
            // Decelerate
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.fixedDeltaTime);
        }

        // Apply movement to rigidbody
        rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);

        // Update animation if you have an animator
        if (animator != null)
        {
            animator.SetFloat("Speed", currentVelocity.magnitude / moveSpeed);
        }
    }
    private Vector3 ConvertToIsometric(Vector3 input)
    {
        Matrix4x4 isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        return isoMatrix.MultiplyVector(input);
    }
}