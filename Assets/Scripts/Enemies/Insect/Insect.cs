using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Insect : Enemy
{
    [SerializeField] private float attackDamage;
    [SerializeField] private float walkingSpeed;
    [SerializeField] private float chasingSpeed;

    private bool alreadyAttacked;

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private PlayerStats playerStats;
    private AudioSource attackSound;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
        playerStats = player.GetComponent<PlayerStats>();
        attackSound = GetComponent<AudioSource>();
    }

    override protected EnemyStates state
    {
        get
        {
            if (Physics.CheckSphere(transform.position, attackRange, whatIsPlayer))
                return EnemyStates.ATTACKING;

            if (Physics.CheckSphere(transform.position, sightRange, whatIsPlayer))
                return EnemyStates.CHASING;

            return EnemyStates.PATROLLING;
        }
    }

    override protected void AttackPlayer()
    {
        transform.LookAt(player.position);
        agent.SetDestination(transform.position);

        if (!alreadyAttacked)
        {
            animator.speed = 4f;
            animator.SetTrigger("Attack");
            attackSound.Play();
            playerStats.TakeDamage(attackDamage);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }

    override protected void ChasePlayer()
    {
        transform.LookAt(player.position);
        agent.SetDestination(player.position);
        agent.isStopped = false;
        agent.speed = chasingSpeed;
        animator.SetBool("IsWalking", true);
        animator.speed = 5f;
    }

    override protected void Patrolling()
    {
        agent.speed = walkingSpeed;
        animator.speed = 1.5f;
        if (!walkPointSet)
            SearchWalkPoint();
        if (walkPointSet && !agent.isStopped)
        {
            animator.SetBool("IsWalking", true);
            agent.SetDestination(walkPoint);

            Vector3 distance = transform.position - walkPoint;
            if (distance.magnitude < 1f)
            {
                agent.isStopped = true;
                animator.SetBool("IsWalking", false);
                IEnumerator WaitAndSearch()
                {
                    yield return new WaitForSeconds(2f);
                    agent.isStopped = false;
                    walkPointSet = false;
                }
                StartCoroutine(WaitAndSearch());
            }
        }
    }

    override protected void Die()
    {
        agent.isStopped = true;
        animator.speed = 1;
        kills.AddKillCount(1);
        animator.SetTrigger("Die");
        Invoke(nameof(EndEnemy), 10f);
    }

    private void EndEnemy()
    {
        Destroy(gameObject);
    }
}
