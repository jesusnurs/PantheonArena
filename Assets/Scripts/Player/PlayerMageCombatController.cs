using UnityEngine;

namespace Player
{
    public class PlayerMageCombatController : PlayerCombatController
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float projectileSpeed;

        private Camera cam;
        private Vector3 destination;

        private float timeToFire;

        private void Update()
        {
            // Don't process input if game is paused
            if (Time.timeScale == 0 || _playerController.IsDead)
                return;

            if (Input.GetMouseButton(0) && Time.time > timeToFire)
            {
                timeToFire = Time.time + _statsData.BasicCooldown;
                ShootProjectile();
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
            var projectile = Instantiate(projectilePrefab, firePoint.position, targetRotation);

            Vector3 velocity = destination * projectileSpeed;
            projectile.GetComponent<Rigidbody>().velocity = velocity;
        }
    }
}