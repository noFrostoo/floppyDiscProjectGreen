using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthSystem
{
    public event EventHandler OnDeath;
    public event EventHandler<OnHealedArgs> OnHealed;

    public class OnHealedArgs : EventArgs
    {
        public int excessHealth;
    }

    public event EventHandler OnDamaged;
    public event EventHandler<OnMaxHealthChangedArgs> OnMaxHealthChanged;
    public class OnMaxHealthChangedArgs : EventArgs
    {
        public int excessHealth;
    }
    private int maxHealth;
    private int currentHealth;

    public HealthSystem(int maxHealth, int health = -1)
    {
        this.maxHealth = maxHealth;
        if(health < 0) this.currentHealth = maxHealth;
        else this.currentHealth = health;
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public bool isDead()
    {
        return currentHealth <= 0;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if(currentHealth <= 0)
            currentHealth = 0;
        if(isDead()) Die();
        OnDamaged?.Invoke(this, EventArgs.Empty);
    }

    public void Die() 
    {
        OnDeath?.Invoke(this, EventArgs.Empty);
    }

    public void ChangeMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        int exessHealth = currentHealth - maxHealth; 
        if(currentHealth < maxHealth)
        {
            currentHealth = maxHealth;
        }
        OnMaxHealthChanged?.Invoke(this, new OnMaxHealthChangedArgs{
            excessHealth = exessHealth
        });
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        int exessHealth = currentHealth - maxHealth; 
        if(currentHealth > maxHealth) 
            currentHealth = maxHealth;

    }

    public void HealFull()
    {
        currentHealth = maxHealth;
        OnHealed?.Invoke(this, new OnHealedArgs{
            excessHealth = 0
        });
    }

    public void SetHealth(int amount)
    {
        if((amount >= 0) && (amount <= maxHealth))
        {
            if( amount > currentHealth)
            {
                int excessHealth = amount - currentHealth;
                currentHealth = amount;
                OnHealed?.Invoke(this, new OnHealedArgs{
                excessHealth = excessHealth
                });
            }
            else
            {
                int damageAmount = currentHealth - amount;
                currentHealth = amount;
                OnDamaged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}