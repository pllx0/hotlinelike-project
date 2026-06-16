using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem; // Obrigatório para o novo sistema!

public class playermovement : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 18f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 0.6f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDir = Vector2.right;

    private bool isDashing;
    private bool canDash = true;
    public bool IsInvincible { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // 1. Chamado pelo Player Input quando as teclas WASD/Analógico mudam
    public void OnMovement(InputAction.CallbackContext context)
    {
        // Lê o Vector2 do input e já normaliza
        moveInput = context.ReadValue<Vector2>().normalized;

        // Guarda a última direção válida para o dash
        if (moveInput.sqrMagnitude > 0.01f)
        {
            lastMoveDir = moveInput;
        }
    }

    // 2. Chamado pelo Player Input quando o botão de Dash (Mouse 2 / Shift) é pressionado
    public void OnDash(InputAction.CallbackContext context)
    {
        // context.started garante que a ação só roda UMA vez no momento do clique
        if (context.started && canDash && !isDashing)
        {
            StartCoroutine(DoDash());
        }
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            // Movimentação normal baseada no input guardado pelo evento
            rb.linearVelocity = moveInput * moveSpeed;
        }
    }

    private IEnumerator DoDash()
    {
        isDashing = true;
        canDash = false;
        IsInvincible = true;

        Vector2 dashDir = lastMoveDir;
        float elapsed = 0f;

        while (elapsed < dashDuration)
        {
            rb.linearVelocity = dashDir * dashSpeed;
            elapsed += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
        IsInvincible = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}