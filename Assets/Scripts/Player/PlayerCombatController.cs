using Data;
using UnityEngine;

namespace Player
{
    public class PlayerCombatController : MonoBehaviour
    {
        protected PlayerController _playerController;
        protected PlayerStatsData _statsData;
        public void Init(PlayerStatsData statsData, PlayerController playerController)
        {
            _statsData = statsData;
            _playerController = playerController;
        }
    }
}
