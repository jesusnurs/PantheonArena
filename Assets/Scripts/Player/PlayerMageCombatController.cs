using System;
using UnityEngine;

namespace Player
{
    public class PlayerMageCombatController : PlayerCombatController
    {
        [SerializeField] private GameObject fireballPrefab;
        [SerializeField] private GameObject fireballBoostedPrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float projectileSpeed;

        private Camera cam;
        private Vector3 destination;

        private float _basicAttackCooldown;
        private float _skillCooldown;
        private float _skillDuration;

        private bool _isAttackBoost => Time.time <= _skillDuration;

        private void Update()
        {
            // Don't process input if game is paused
            if (Time.timeScale == 0 || _playerController.IsDead)
                return;

            if (Input.GetMouseButton(0) && Time.time > _basicAttackCooldown)
            {
                _basicAttackCooldown = Time.time + _statsData.BasicAttackCooldown;
                ShootProjectile();
            }
            
            if (Input.GetKeyDown(KeyCode.E) && Time.time > _skillCooldown)
            {
                _skillCooldown = Time.time + _statsData.SkillCooldown;
                _skillDuration = Time.time + (_statsData.SkillCooldown / 4);
                UseSkill();
            }
        }

        private void ShootProjectile()
        {
            var (success, targetPosition) = _playerController.GetMousePosition();
            if (success)
            {
                Vector3 direction = (targetPosition - firePoint.position);
                direction.Normalize();
                destination = direction;
                InstantiateProjectile();
            }
        }

        private void InstantiateProjectile()
        {
            var targetRotation = Quaternion.LookRotation(destination);
            var projectile = Instantiate(_isAttackBoost ? fireballBoostedPrefab : fireballPrefab, firePoint.position, targetRotation);

            Vector3 velocity = destination * projectileSpeed;
            projectile.GetComponent<Rigidbody>().velocity = velocity;
        }

        private void UseSkill()
        {
            _statsHUD.SetAttackBoostStatus(_skillCooldown);
        }
    }
}