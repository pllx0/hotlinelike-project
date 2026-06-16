using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Vida")]
    [Tooltip("1 = morre em 1 hit. Valores maiores = inimigo mais resistente (ex: com colete).")]
    [SerializeField] private int maxHP = 1;

    private int currentHP;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;

        if (currentHP <= 0)
            Die();
    }

    private void Die()
    {
        // TODO: spawnar efeito de morte, tocar SFX, etc.
        Destroy(gameObject);
    }
}