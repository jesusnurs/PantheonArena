using Data;
using UnityEngine;

namespace Player
{
    public class PlayerCombatController : MonoBehaviour
    {
        protected PlayerStatsData _statsData;
        protected PlayerController _playerController;
        protected HealthController _healthController;
        protected CombatStatsHUD _statsHUD;
        public void Init(PlayerStatsData statsData, PlayerController playerController, HealthController healthController, CombatStatsHUD combatStatsHUD)
        {
            _statsData = statsData;
            _playerController = playerController;
            _healthController = healthController;
            _statsHUD = combatStatsHUD;
        }
    }
}
