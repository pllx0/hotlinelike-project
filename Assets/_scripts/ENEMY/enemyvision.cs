using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [Header("Cone de visão")]
    [SerializeField] private float viewRadius = 5f;
    [SerializeField][Range(0, 360)] private float viewAngle = 90f;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask playerMask;

    public Vector2 FacingDirection { get; set; } = Vector2.right;

    public bool CanSeePlayer { get; private set; }
    public Transform VisiblePlayer { get; private set; }

    private void Update()
    {
        CanSeePlayer = false;

        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, viewRadius, playerMask);

        foreach (var target in targets)
        {
            Vector2 dirToTarget = (target.transform.position - transform.position).normalized;

            float angle = Vector2.Angle(FacingDirection, dirToTarget);

            if (angle < viewAngle / 2f)
            {
                float dist = Vector2.Distance(transform.position, target.transform.position);

                RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToTarget, dist, obstacleMask);

                if (hit.collider == null)
                {
                    CanSeePlayer = true;
                    VisiblePlayer = target.transform;
                    break;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = CanSeePlayer ? Color.red : Color.yellow;

        Vector3 leftBoundary = DirFromAngle(-viewAngle / 2f);
        Vector3 rightBoundary = DirFromAngle(viewAngle / 2f);

        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewRadius);
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }

    private Vector3 DirFromAngle(float angleOffset)
    {
        float baseAngle = Mathf.Atan2(FacingDirection.y, FacingDirection.x) * Mathf.Rad2Deg;
        float finalAngle = (baseAngle + angleOffset) * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(finalAngle), Mathf.Sin(finalAngle));
    }
}