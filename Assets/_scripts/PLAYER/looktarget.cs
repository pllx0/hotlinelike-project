using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLookTarget : MonoBehaviour
{
    [SerializeField] private float lookOffset = 2f;
    [SerializeField] private float smoothSpeed = 5f;

    private void Update()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0f;

        Vector3 dirToMouse = (mouseWorldPos - transform.parent.position).normalized;
        Vector3 targetPos = transform.parent.position + dirToMouse * lookOffset;

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
    }
}