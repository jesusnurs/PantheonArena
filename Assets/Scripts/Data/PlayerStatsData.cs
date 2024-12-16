using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [CreateAssetMenu(fileName = "PlayerAttackData", menuName = "Character Data/PlayerAttackData")]
    public class PlayerStatsData : ScriptableObject
    {
        [Header("Health")]
        [SerializeField] private float maxHealth;
        
        [Header("Damage")]
        [SerializeField] private float basicDamage;
        [SerializeField] private LayerMask enemyLayer;
        
        [Header("Cooldown")]
        [SerializeField] private float basicAttackCooldown;
        [SerializeField] private float skillCooldown;
        
        
        public float MaxHealth => maxHealth;
        public float BasicDamage => basicDamage;
        public float BasicAttackCooldown => basicAttackCooldown;
        public float SkillCooldown => skillCooldown;
        public LayerMask EnemyLayer => enemyLayer;
    }
}
