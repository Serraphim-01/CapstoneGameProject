using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public enum Type { Common, Elite, Boss }
    public Type enemyType = Type.Common;

    public int maxHealth = 100;
    private int currentHealth;
    private Animator animator;
    public bool IsDead { get; private set; }

    private bool hasRegistered = false;
    private bool hasUnregistered = false;
    public int CurrentHealth => currentHealth;


    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponentInChildren<Animator>();


        EnemyManager.Instance?.RegisterEnemy(this);
    }

    public void TakeDamage(int amount)
    {
        if (IsDead) return;

        currentHealth -= amount;
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (IsDead) return;
        IsDead = true;

        animator.SetTrigger("Die");

        var enemyBehavior = GetComponent<EnemyBehavior>();
        if (enemyBehavior != null)
            enemyBehavior.enabled = false;

        GetComponent<Collider>().enabled = false;

        if (!hasUnregistered)
        {
            EnemyManager.Instance?.UnregisterEnemy(this);
            hasUnregistered = true;
        }

        StartCoroutine(DelayedDestroy());
    }

    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
