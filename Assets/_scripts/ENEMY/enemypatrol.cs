using UnityEngine;

public class enemypatrol : MonoBehaviour
{
    private enum State { Patrol, Chase, Attack }

    [Header("Detecção")]
    [SerializeField] private float detectRange = 5f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private EnemyVision vision;

    [Header("Movimento")]
    [SerializeField] private float chaseSpeed = 3f;

    [Header("Patrulha")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolSpeed = 1.5f;
    private int currentPatrolIndex;

    private State currentState = State.Patrol;
    private Rigidbody2D rb;
    private Transform player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("player").transform;
    }

    private void Update()
    {
        vision.FacingDirection = rb.linearVelocity.normalized;
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        bool canSeePlayer = vision.CanSeePlayer;

        if (canSeePlayer && distToPlayer <= attackRange)
            currentState = State.Attack;
        else if (canSeePlayer)
            currentState = State.Chase;
        else if (currentState != State.Patrol && distToPlayer > detectRange * 1.3f)
            currentState = State.Patrol;

        switch (currentState)
        {
            case State.Patrol: Patrol(); break;
            case State.Chase: Chase(); break;
            case State.Attack: Attack(); break;
        }
    }


    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform target = patrolPoints[currentPatrolIndex];
        MoveTowards(target.position, patrolSpeed);

        if (Vector2.Distance(transform.position, target.position) < 0.2f)
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    private void Chase()
    {
        MoveTowards(player.position, chaseSpeed);
    }

    private void Attack()
    {
        rb.linearVelocity = Vector2.zero;
    }

    private void MoveTowards(Vector2 target, float speed)
    {
        Vector2 dir = (target - rb.position).normalized;
        rb.linearVelocity = dir * speed;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}