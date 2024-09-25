using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float maxHealth;
    protected float health;
    [SerializeField] protected float sightRange;
    [SerializeField] protected float attackRange;

    // Patrolling
    [SerializeField] protected float walkPointRange;
    protected Vector3 walkPoint;
    protected bool walkPointSet;

    // Attacking
    [SerializeField] protected float timeBetweenAttacks;

    [SerializeField] protected HealthBar healthBar;
    [SerializeField] protected LayerMask whatIsPlayer;
    [SerializeField] protected LayerMask whatIsGround;

    protected enum EnemyStates { PATROLLING, CHASING, ATTACKING }

    private void Start()
    {
        health = maxHealth;
        healthBar.SetSliderMax(maxHealth);
    }

    private void Update()
    {
        if (!isAlive)
            return;

        EnemyStates enemyState = state;
        if (enemyState == EnemyStates.ATTACKING)
            AttackPlayer();
        else if (enemyState == EnemyStates.CHASING)
            ChasePlayer();
        else
            Patrolling();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.SetSlider(health);
        if (health <= 0)
            Die();
    }

    protected void SearchWalkPoint()
    {
        float x = transform.position.x + Random.Range(-walkPointRange, walkPointRange);
        float z = transform.position.z + Random.Range(-walkPointRange, walkPointRange);
        RaycastHit hit;
        Ray ray = new Ray(new Vector3(x, 300f, z), Vector3.down);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, whatIsGround))
        {
            walkPointSet = true;
            walkPoint = new Vector3(x, hit.point.y, z);
        }
    }

    protected abstract EnemyStates state { get; }
    protected abstract void Patrolling();
    protected abstract void AttackPlayer();
    protected abstract void ChasePlayer();
    protected abstract void Die();

    public bool isAlive => health > 0;
}
