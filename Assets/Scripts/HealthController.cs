using System;
using UnityEngine;

public class HealthController : MonoBehaviour, IDamageable
{
    public Action OnHealthChanged;
    public Action OnDeath;
    
    //In case of NoInitialization
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _maxHealth;

    public float CurrentHealth
    {
        set
        {
            _currentHealth = value;
            OnHealthChanged?.Invoke();
        }
        get => _currentHealth;
    }

    public float MaxHealth
    {
        set => _maxHealth = value;
        get => _maxHealth;
    }

    public void Init(float health, float maxHealth)
    {
        CurrentHealth = health;
        MaxHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            Death();
        }
    }

    public void TakeHeal(float heal)
    {
        CurrentHealth += heal;
        
        if(CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }

    private void Death()
    {
        Debug.Log("Death");
        OnDeath?.Invoke();
    }
}
