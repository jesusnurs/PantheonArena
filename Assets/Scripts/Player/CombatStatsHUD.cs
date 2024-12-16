using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class CombatStatsHUD : MonoBehaviour
    {
        private HealthController _healthController;
        
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private TextMeshProUGUI _nicknameText;
        
        [Header("Skills")]
        [SerializeField] private GameObject skillsPanel;
        [SerializeField] private GameObject healStatus;
        [SerializeField] private GameObject attackBoostStatus;
        [SerializeField] private TextMeshProUGUI healStatusText;
        [SerializeField] private TextMeshProUGUI attackBoostText;

        private float _healCooldown;
        private float _attackBoostCooldown;
        
        private Camera _mainCamera;

        public void Init(HealthController healthController)
        {
            _healthController = healthController;

            _healthController.OnHealthChanged += UpdateHealthVisuals;
            _nicknameText.text = PlayerPrefs.GetString("nickname","Player");
        }

        private void Start()
        {
            _mainCamera = Camera.main;
            transform.GetComponent<Canvas>().worldCamera = _mainCamera;
        }

        private void Update()
        {
            transform.rotation = Quaternion.LookRotation(transform.position - _mainCamera.transform.position) ;
        }

        public void SetHealStatus(float healCooldown)
        {
            _healCooldown = healCooldown;
            skillsPanel.SetActive(true);
            healStatus.SetActive(true);
            InvokeRepeating(nameof(UpdateHealStatus), 0,1);
        }
        
        public void SetAttackBoostStatus(float attackBoostCooldown)
        {
            _attackBoostCooldown = attackBoostCooldown;
            skillsPanel.SetActive(true);
            attackBoostStatus.SetActive(true);
            InvokeRepeating(nameof(UpdateAttackBoostStatus), 0,1);
        }

        private void UpdateHealthVisuals()
        {
            _healthSlider.value = _healthController.CurrentHealth/_healthController.MaxHealth;
        }

        private void UpdateHealStatus()
        {
            float time = _healCooldown - Time.time;
            if(time <= 0)
            {
                healStatus.SetActive(false);
                skillsPanel.SetActive(attackBoostStatus.activeSelf);
                CancelInvoke(nameof(UpdateHealStatus));
            }
            
            healStatusText.text = $"{TimeSpan.FromSeconds(time).Seconds}s";
        }
        
        private void UpdateAttackBoostStatus()
        {
            float time = _attackBoostCooldown - Time.time;
            if(time <= 0)
            {
                attackBoostStatus.SetActive(false);
                skillsPanel.SetActive(healStatus.activeSelf);
                CancelInvoke(nameof(UpdateAttackBoostStatus));
            }
            
            attackBoostText.text = $"{TimeSpan.FromSeconds(time).Seconds}s";
        }
    }
}
