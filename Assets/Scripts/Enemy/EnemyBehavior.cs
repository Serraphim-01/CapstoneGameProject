using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    private Transform player;
    public float detectionRadius = 20f;
    public float stopDistance = 2f;
    public float timeBetweenAttacks = 2f;
    public int damage = 10;

    private NavMeshAgent agent;
    private Animator animator;
    private PlayerHealth playerHealth;

    private bool isChasing;
    private bool alreadyAttacked;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Auto-find player
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                playerHealth = player.GetComponent<PlayerHealth>();
            }
        }
        else
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        if (player == null || playerHealth == null || playerHealth.IsDead) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRadius && distance > stopDistance)
        {
            if (!isChasing)
            {
                isChasing = true;
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsSensing", false);
            }

            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else if (distance <= stopDistance)
        {
            agent.isStopped = true;

            animator.SetBool("IsWalking", false);
            animator.SetBool("IsSensing", false);

            FacePlayer();

            if (!alreadyAttacked)
            {
                AttackPlayer();
            }
        }
        else
        {
            if (isChasing)
            {
                isChasing = false;
                agent.ResetPath();
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsSensing", true);
            }
        }
    }

    void AttackPlayer()
    {
        int attackIndex = Random.Range(0, 2);
        animator.SetInteger("AttackIndex", attackIndex);
        animator.SetTrigger("Attack");

        alreadyAttacked = true;
        Invoke(nameof(DealDamage), 1f); // Delay matches the hit moment in animation
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }

    void DealDamage()
    {
        if (playerHealth != null)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            if (dist <= stopDistance + 0.5f) // Extra buffer in case player moves slightly
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Enemy has dealt Damage: " + damage);
            }
        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }

    void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}
