using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Ataque")]
    [SerializeField] private float attackCooldown = 1f;

    private bool canAttack = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (!canAttack) return;
        if (!other.CompareTag("player")) return;

        PlayerHealth health = other.GetComponent<PlayerHealth>();
        
        if (health == null) return;

        health.TakeDamage();

        canAttack = false;
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    private void ResetAttack()
    {
        canAttack = true;
    }
}