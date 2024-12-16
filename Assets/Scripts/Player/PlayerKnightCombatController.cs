using System;
using UnityEngine;

namespace Player
{
    public class PlayerKnightCombatController : PlayerCombatController
    {
        [SerializeField] private Transform _attackPoint;
        [SerializeField] private float _attackRadius = 2.0f;
        
        private Camera _cam;
        private Animator _animator;
        
        private float _basicAttackCooldown;
        private float _skillCooldown;
        private float _healAmount => _statsData.MaxHealth / 4;
        
        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
        }
        
        private void Update()
        {
            // Don't process input if game is paused
            if (Time.timeScale == 0 || _playerController.IsDead)
                return;

            if (Input.GetMouseButton(0) && Time.time > _basicAttackCooldown)
            {
                _basicAttackCooldown = Time.time + _statsData.BasicAttackCooldown;
                MeleeAttack();
            }
            
            if (Input.GetKeyDown(KeyCode.E) && Time.time > _skillCooldown)
            {
                _skillCooldown = Time.time + _statsData.SkillCooldown;
                UseSkill();
            }
        }

        private void MeleeAttack()
        {
            _animator.SetTrigger("BasicAttack");
            
            PerformAttack();
        }

        private void PerformAttack()
        {
            Collider[] hitEnemies = Physics.OverlapSphere(_attackPoint.position, _attackRadius, _statsData.EnemyLayer);

            foreach (var enemy in hitEnemies)
            {
                if (enemy.gameObject != gameObject)
                {
                    var damageable = enemy.GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        damageable.TakeDamage(_statsData.BasicDamage);
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_attackPoint == null)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_attackPoint.position, _attackRadius);
        }

        private void UseSkill()
        {
            _healthController.TakeHeal(_healAmount);
            _statsHUD.SetHealStatus(_skillCooldown);
        }
    }
}

public interface IDamageable
{
    void TakeDamage(float damage);
    void TakeHeal(float heal);
}
