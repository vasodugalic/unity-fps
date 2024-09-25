using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Alien : Enemy
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float fieldOfView;

    private bool alreadyAttacked;

    private Transform gunBarrel;
    private GameObject bullet;
    private Transform player;
    private PlayerStats playerStats;
    private NavMeshAgent agent;
    private Animator animator;
    private AudioSource attackSound;
    private Global global;
    private Kills kills;
    private EnemySpawn enemySpawn;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerStats = player.GetComponent<PlayerStats>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        attackSound = GetComponent<AudioSource>();
        gunBarrel = transform.Find("Gun Barrel");
        bullet = GameObject.FindWithTag("Global").GetComponent<Global>().bullet;
        global = GameObject.FindWithTag("Global").GetComponent<Global>();
        kills = GameObject.FindWithTag("Global").GetComponent<Kills>();
        enemySpawn = GameObject.FindWithTag("Spawner").GetComponent<EnemySpawn>();
    }

    override protected EnemyStates state
    {
        get
        {
            if (IsPlayerInView(attackRange))
                return EnemyStates.ATTACKING;

            if (IsPlayerInView(sightRange))
                return EnemyStates.CHASING;

            return EnemyStates.PATROLLING;
        }
    }

    private bool IsPlayerInView(float range)
    {
        if (Vector3.Distance(player.position, transform.position) < range)
        {
            Vector3 targetDirection = player.position - transform.position;
            float angleToPlayer = Vector3.Angle(transform.forward, targetDirection);
            if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
            {
                Ray ray = new Ray(transform.position, targetDirection);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                    if (hit.transform.CompareTag("Player"))
                        return true;
            }
        }

        return false;
    }

    override protected void Patrolling()
    {
        animator.SetBool("IsChasing", false);

        if (!walkPointSet)
            SearchWalkPoint();
        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distance = transform.position - walkPoint;
        if (distance.magnitude < 1f)
            walkPointSet = false;
    }

    override protected void ChasePlayer()
    {
        animator.SetBool("IsChasing", true);
        agent.SetDestination(player.position);
        transform.LookAt(player.transform.position);
    }

    override protected void AttackPlayer()
    {
        animator.SetBool("IsChasing", true);
        transform.LookAt(player);
        agent.SetDestination(transform.position);

        if (!alreadyAttacked)
        {
            animator.SetTrigger("Shoot");
            attackSound.Play();
            GameObject newBullet = Instantiate(bullet, gunBarrel.position, transform.rotation * Quaternion.Euler(90f, 0f, 0f));
            Vector3 shootDirection = (player.transform.position - gunBarrel.transform.position + Vector3.up).normalized;
            newBullet.GetComponent<Rigidbody>().velocity = shootDirection * bulletSpeed;
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
        animator.ResetTrigger("Shoot");
    }

    override protected void Die()
    {
        agent.isStopped = true;
        animator.SetTrigger("Die");
        kills.AddKillCount(1);
        enemySpawn.DeleteEnemy();
        Invoke(nameof(EndEnemy), 10f);
    }

    void EndEnemy()
    {
        Destroy(gameObject);
    }
}
