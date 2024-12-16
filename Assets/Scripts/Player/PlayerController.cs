using Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerMovementData _movementData;
        private HealthController _healthController;

        private Vector3 _moveDirection;
        private Vector3 _currentVelocity;
        private Animator _animator;
        private Rigidbody _rb;
        private Camera _mainCamera;
        
        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float groundCheckDistance = 0.2f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float fallMultiplier = 2.5f;

        private bool _isGrounded;
        private bool _isDead;
        
        public bool IsDead => _isDead;

        public void Init(PlayerMovementData movementData, HealthController healthController)
        {
            _movementData = movementData;
            _healthController = healthController;

            _healthController.OnDeath += Death;
        }

        private void Start()
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                gameObject.SetActive(false);
            }
            _rb = GetComponent<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if(_isDead)
                return;
            
            CheckGrounded();
            HandleInput();
            Aim();
        }

        private void FixedUpdate()
        {
            if(_isDead)
                return;
            
            ApplyMovement();
            if (_rb.velocity.y < 0)
            {
                _rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
            }
        }

        private void CheckGrounded()
            {
                Debug.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down * groundCheckDistance, Color.red);
                
                _isGrounded = Physics.SphereCast(
                    transform.position + Vector3.up * 0.1f,
                    0.2f,
                    Vector3.down,
                    out var hit,
                    groundCheckDistance,
                    groundLayer
                );
            }
        private void HandleInput()
        {
            // Get movement input
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");

            // Convert input to isometric direction
            var input = new Vector3(horizontal, 0f, vertical).normalized;
            _moveDirection = ConvertToIsometric(input);
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }
        
        private void Jump()
        {

            if (!_isGrounded)
            {
                Debug.Log("Not grounded");
                return;
            }
            Debug.Log("Jumping!");

            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                
            if (_animator != null)
            {
                _animator.SetTrigger("Jump");
            }
        }
            
        private void ApplyMovement()
        {
            // Apply gravity multiplier for smoother falling
            if (_rb.velocity.y < 0)
            {
                _rb.velocity += Vector3.up * Physics.gravity.y * (_movementData.FallMultiplier - 1) * Time.fixedDeltaTime;
            }

            // Apply movement
            if (_moveDirection.magnitude > 0.1f)
            {
                // Accelerate
                _currentVelocity = Vector3.Lerp(_currentVelocity, _moveDirection * _movementData.MoveSpeed,
                    _movementData.Acceleration * Time.fixedDeltaTime);
            }
            else
            {
                // Decelerate
                _currentVelocity = Vector3.Lerp(_currentVelocity, Vector3.zero, _movementData.Deceleration * Time.fixedDeltaTime);
            }

            // Apply movement to rigidbody
            _rb.MovePosition(_rb.position + _currentVelocity * Time.fixedDeltaTime);

            // Update animation if you have an animator
            if (_animator != null)
            {
                _animator.SetFloat("Speed", _currentVelocity.magnitude / _movementData.MoveSpeed);
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
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _movementData.RotationSpeed * Time.deltaTime);
            }
        }

        public (bool success, Vector3 position) GetMousePosition()
        {
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, _movementData.GroundLayer))
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
     
        private void Death()
        {
            _animator.SetTrigger("Death");
            _isDead = true;

            //TODO UI and Respawn button
        }
    }
}
