using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TrainingBagUI : MonoBehaviour
{
    [SerializeField] private HealthController healthController;
    
    [SerializeField] private TextMeshProUGUI receivedDamageText;
    [SerializeField] private Slider healthBar;
    private Camera cam;
    
    private float previousHealthCount;
    
    private void Awake()
    {
        cam = Camera.main;
        
        healthController.OnHealthChanged += UpdateHealthBar;
        previousHealthCount = healthController.CurrentHealth;
    }
    
    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }

    private void ShowFloatDamageText()
    {
        receivedDamageText.text = (previousHealthCount - healthController.CurrentHealth).ToString();
        previousHealthCount = healthController.CurrentHealth;

        receivedDamageText.transform.position = transform.position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
        receivedDamageText.gameObject.SetActive(true);
        receivedDamageText.transform.DOScale(Vector3.one, 0.3f).From(Vector3.one * 0.7f);
        receivedDamageText.transform.DOLocalMoveY(3, 0.3f).
            OnComplete( () => receivedDamageText.gameObject.SetActive(false));
    }

    private void UpdateHealthBar()
    {
        healthBar.value = healthController.CurrentHealth / healthController.MaxHealth;
        ShowFloatDamageText();
    }
}
