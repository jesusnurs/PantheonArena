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
        
        private Camera mainCamera;

        public void Init(HealthController healthController)
        {
            _healthController = healthController;

            _healthController.OnHealthChanged += UpdateHUDVisuals;
            _nicknameText.text = PlayerPrefs.GetString("nickname","Player");
        }

        private void Start()
        {
            mainCamera = Camera.main;
            transform.GetComponent<Canvas>().worldCamera = mainCamera;
        }

        private void Update()
        {
            transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position) ;
        }

        private void UpdateHUDVisuals()
        {
            _healthSlider.value = _healthController.CurrentHealth/_healthController.MaxHealth;
        }
    }
}
