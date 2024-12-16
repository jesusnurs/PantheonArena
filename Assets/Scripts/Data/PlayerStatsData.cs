using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "PlayerAttackData", menuName = "Character Data/PlayerAttackData")]
    public class PlayerStatsData : ScriptableObject
    {
        [Header("Health")]
        [SerializeField] private float maxHealth;
        
        [Header("Damage")]
        [SerializeField] private float basicDamage;
        [SerializeField] private float skillOneDamage;
        [SerializeField] private float skillTwoDamage;
        [SerializeField] private float skillThreeDamage;
        [SerializeField] private float ultimateDamage;
        
        [Header("Cooldown")]
        [SerializeField] private float basicCooldown;
        [SerializeField] private float skillOneCooldown;
        [SerializeField] private float skillTwoCooldown;
        [SerializeField] private float skillThreeCooldown;
        [SerializeField] private float ultimateCooldown;
        
        [SerializeField] private LayerMask enemyLayer;
        
        public float MaxHealth => maxHealth;
        
        public float BasicDamage => basicDamage;
        public float SkillOneDamage => skillOneDamage;
        public float SkillTwoDamage => skillTwoDamage;
        public float SkillThreeDamage => skillThreeDamage;
        public float UltimateDamage => ultimateDamage;

        public float BasicCooldown => basicCooldown;
        public float SkillOneCooldown => skillOneCooldown;
        public float SkillTwoCooldown => skillTwoCooldown;
        public float SkillThreeCooldown => skillThreeCooldown;
        public float UltimateCooldown => ultimateCooldown;

        public LayerMask EnemyLayer => enemyLayer;
    }
}
