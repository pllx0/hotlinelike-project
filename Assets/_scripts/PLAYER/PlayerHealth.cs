using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private playermovement playerMovement;

    private bool isDead;

    public static event Action OnPlayerDied;

    public void TakeDamage()
    {
        
        if (isDead) return;
        if (playerMovement.IsInvincible) return;

        Die();
    }

    private void Die()
    {
        isDead = true;
        
        OnPlayerDied?.Invoke();
    }
}