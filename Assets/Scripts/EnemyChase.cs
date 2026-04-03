using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyChase : MonoBehaviour
{
    public float runSpeed = 6f;
    public float attackRange = 1.6f;
    public int attackDamage = 10;
    public float attackCooldownSeconds = 0.6f;

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private PlayerHealth playerHealth;
    private Collider playerCollider;
    private float nextAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.stoppingDistance = 0f;
        agent.autoBraking = false;

        FindPlayerRefs();
        StartCoroutine(EnableAgentDelay());
    }

    IEnumerator EnableAgentDelay()
    {
        yield return new WaitForSeconds(0.1f);
        agent.enabled = true;
    }

    void Update()
    {
        if (player == null || playerHealth == null)
        {
            FindPlayerRefs();
        }

        if (player == null || !agent.enabled || !agent.isOnNavMesh)
        {
            SetRunAnimation(false);
            return;
        }

        agent.stoppingDistance = 0f;
        agent.speed = runSpeed;
        agent.SetDestination(player.position);

        SetRunAnimation(true);
        TryAttack();
    }

    void FindPlayerRefs()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null) return;

        player = playerObject.transform;
        playerHealth = playerObject.GetComponentInChildren<PlayerHealth>();
        if (playerHealth == null)
        {
            playerHealth = playerObject.GetComponentInParent<PlayerHealth>();
        }

        playerCollider = playerObject.GetComponentInChildren<Collider>();
        if (playerCollider == null)
        {
            playerCollider = playerObject.GetComponentInParent<Collider>();
        }
    }

    void SetRunAnimation(bool running)
    {
        if (animator == null) return;

        float speedValue = running ? 1f : 0f;
        animator.SetFloat("Speed", speedValue, 0.1f, Time.deltaTime);
        animator.SetBool("Sprint", running);
    }

    void TryAttack()
    {
        if (Time.time < nextAttackTime) return;

        Vector3 enemyPosition = transform.position;
        Vector3 playerPoint = playerCollider != null ? playerCollider.ClosestPoint(enemyPosition) : player.position;

        enemyPosition.y = 0f;
        playerPoint.y = 0f;

        float distance = Vector3.Distance(enemyPosition, playerPoint);

        float pathDistance = float.PositiveInfinity;
        if (!agent.pathPending && agent.hasPath)
        {
            pathDistance = agent.remainingDistance;
        }

        float effectiveDistance = Mathf.Min(distance, pathDistance);
        if (effectiveDistance > attackRange) return;

        nextAttackTime = Time.time + attackCooldownSeconds;

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }
}
