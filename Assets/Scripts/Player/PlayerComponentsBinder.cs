using Data;
using UnityEngine;

namespace Player
{
    public class PlayerComponentsBinder : MonoBehaviour
    {
        [Header("Data")] 
        [SerializeField] private PlayerStatsData playerStatsData;
        [SerializeField] private PlayerMovementData playerMovementData;

        [Header("Components")] 
        [SerializeField] private HealthController healthController;
        [SerializeField] private CombatStatsHUD statsHUD;
        [SerializeField] private PlayerCombatController combatController;
        [SerializeField] private PlayerController playerController;

        [Header("Test")] 
        public bool deathTest;

        private void Awake()
        {
            healthController.Init(playerStatsData.MaxHealth, playerStatsData.MaxHealth);
            statsHUD.Init(healthController);
            playerController.Init(playerMovementData, healthController);
            combatController.Init(playerStatsData, playerController, healthController, statsHUD);
            
            Invoke(nameof(Death),5f);
        }

        private void Death()
        {
            if(deathTest)
                healthController.TakeDamage(1000);
        }
    }
}