using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "PlayerMovementData", menuName = "Character Data/PlayerMovementData")]
    public class PlayerMovementData : ScriptableObject
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 20f;

        [Header("Physics Settings")]
        [SerializeField] private float acceleration = 8f;
        [SerializeField] private float deceleration = 12f;
        [SerializeField] private float jumpForce = 5f;

        [Header("Ground Detection")]
        [SerializeField] private float groundCheckDistance = 0.2f;
        [SerializeField] private LayerMask groundLayer;
        
        [Header("Gravity Settings")]
        [SerializeField] private float fallMultiplier = 2.5f;
        
        public float MoveSpeed => moveSpeed;
        public float RotationSpeed => rotationSpeed;
        public float Acceleration => acceleration;
        public float Deceleration => deceleration;
        public float JumpForce => jumpForce;
        public float GroundCheckDistance => groundCheckDistance;
        public LayerMask GroundLayer => groundLayer;
        public float FallMultiplier => fallMultiplier;
    }
}
