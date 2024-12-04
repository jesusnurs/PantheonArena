using Data;
using DG.Tweening;
using Player;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] private PlayerAttackData attackData;
    
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed;
    
    private Camera cam;
    private Vector3 destination;
    
    private float timeToFire;
    
    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && Time.time > timeToFire)
        {
            timeToFire = Time.time + attackData.BasicCooldown;
            ShootProjectile();
        }
    }

    private void ShootProjectile()
    {
        var (success, position) = playerController.GetMousePosition();
        if (success)
        {
            Vector3 direction = (position - transform.position);
            direction.y = 0; 
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
