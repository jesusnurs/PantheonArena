using Data;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private PlayerMovementData movementData;

        private bool isGrounded;
        private Vector3 moveDirection;
        private Vector3 currentVelocity;
        private Animator animator;
        private Rigidbody rb;
        private Camera mainCamera;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<Animator>();
            mainCamera = Camera.main;
        }

        private void Update()
        {
            CheckGrounded();
            HandleInput();
            Aim();
        }

        private void FixedUpdate()
        {
            ApplyMovement();
        }

        private void CheckGrounded()
        {
            isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down,
                movementData.GroundCheckDistance, movementData.GroundLayer);
        }

        private void HandleInput()
        {
            // Get movement input
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            // Convert input to isometric direction
            Vector3 input = new Vector3(horizontal, 0f, vertical).normalized;
            moveDirection = ConvertToIsometric(input);
        }

        private void ApplyMovement()
        {
            // Apply gravity multiplier for smoother falling
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (movementData.FallMultiplier - 1) * Time.fixedDeltaTime;
            }

            // Apply movement
            if (moveDirection.magnitude > 0.1f)
            {
                // Accelerate
                currentVelocity = Vector3.Lerp(currentVelocity, moveDirection * movementData.MoveSpeed,
                    movementData.Acceleration * Time.fixedDeltaTime);
            }
            else
            {
                // Decelerate
                currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, movementData.Deceleration * Time.fixedDeltaTime);
            }

            // Apply movement to rigidbody
            rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);

            // Update animation if you have an animator
            if (animator != null)
            {
                animator.SetFloat("Speed", currentVelocity.magnitude / movementData.MoveSpeed);
            }
        }

        private void Aim()
        {
            var (success, position) = GetMousePosition();

            if (success)
            {
                // Calculate the direction
                var direction = position - transform.position;

                // Ignore the height difference
                direction.y = 0;

                // Smoothly rotate towards the target
                var targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, movementData.RotationSpeed * Time.deltaTime);
            }
        }

        public (bool success, Vector3 position) GetMousePosition()
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, movementData.GroundLayer))
            {
                // The Raycast hit something, return with the position.
                return (success: true, position: hitInfo.point);
            }

            // The Raycast did not hit anything.
            return (success: false, position: Vector3.zero);
        }

        private Vector3 ConvertToIsometric(Vector3 input)
        {
            Matrix4x4 isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
            return isoMatrix.MultiplyVector(input);
        }
    }
}
