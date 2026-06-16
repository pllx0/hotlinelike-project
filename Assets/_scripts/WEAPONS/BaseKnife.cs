using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;

public class WeaponMelee : MonoBehaviour
{
    [Header("Ataque")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackDuration = 0.15f; // tempo que a hitbox fica ativa
    [SerializeField] private float attackCooldown = 0.3f;
    [SerializeField] private Collider2D hitboxCollider; // fica desativado por padrão

    private bool canAttack = true;
    private readonly HashSet<Collider2D> alreadyHit = new();

    private void Awake()
    {
        hitboxCollider.enabled = false;
    }

    // Chamado pelo PlayerInput component (action "Attack" -> método "OnAttack")
    public void OnAttack(InputValue value)
    {
        if (value.isPressed && canAttack)
        {
            StartCoroutine(DoAttack());
        }
    }

    private IEnumerator DoAttack()
    {
        canAttack = false;
        alreadyHit.Clear();
        hitboxCollider.enabled = true;

        yield return new WaitForSeconds(attackDuration);

        hitboxCollider.enabled = false;

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("enemy")) return;
        if (alreadyHit.Contains(other)) return; // evita dano múltiplo no mesmo ataque

        alreadyHit.Add(other);

        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
            enemyHealth.TakeDamage(damage);
    }
}