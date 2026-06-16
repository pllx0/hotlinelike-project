using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private Transform weaponPivot; // objeto filho que gira (carrega a arma)

    public Vector2 AimDirection { get; private set; } = Vector2.right;

    private void Update()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0f;

        AimDirection = (mouseWorldPos - transform.position).normalized;

        float angle = Mathf.Atan2(AimDirection.y, AimDirection.x) * Mathf.Rad2Deg;
        weaponPivot.rotation = Quaternion.Euler(0f, 0f, angle);

        // Vira o sprite do player conforme o lado do mouse (opcional)
        Vector3 scale = transform.localScale;
        scale.y = (AimDirection.x < 0) ? -Mathf.Abs(scale.y) : Mathf.Abs(scale.y);
        transform.localScale = scale;
    }
}