﻿using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject ragDoll;

    [Header("Combat")]
    [SerializeField] string[] attackSkills;
    [SerializeField] float attackCD;
    [SerializeField] float attackRange;
    [SerializeField] float aggroRange;

    [Header("Patrol")]
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] float patrolSpeed;
    [SerializeField] float chaseSpeed;
    [SerializeField] float patrolWaitTime;

    [Header("PowerUp")]
    [SerializeField] float damageIncreasePercent;
    [SerializeField] float speedIncreasePercent;
    [SerializeField] int damageThreshold = 5;

    GameObject player;
    Animator animator;
    NavMeshAgent agent;
    BossDamageDealer damageDealer;
    BossSlash bossSlash;
    float timePassed;
    float powerUpTimer;
    bool isAttacking;
    int currentPatrolIndex;
    bool playerDetected;
    float patrolTimer;
    bool isTakingDamage;
    int damageCounter = 0;
    bool isPoweredUp = false;
    float baseDamage;
    float baseChaseSpeed;
    float powerUpDuration = 20f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        damageDealer = GetComponentInChildren<BossDamageDealer>();
        bossSlash = GetComponent<BossSlash>();

        baseDamage = damageDealer.damage;
        baseChaseSpeed = chaseSpeed;

        if (patrolPoints.Length > 0)
        {
            currentPatrolIndex = 0;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
            agent.speed = patrolSpeed;
        }
    }

    void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude / agent.speed);

        if (player == null)
        {
            HandlePlayerDeath();
            return;
        }

        if (Vector3.Distance(player.transform.position, transform.position) <= aggroRange)
        {
            playerDetected = true;
            transform.LookAt(player.transform);
        }
        else
        {
            playerDetected = false;
            animator.SetBool("Chase", false);
        }

        if (playerDetected)
        {
            HandleCombat();
        }
        else
        {
            HandlePatrol();
        }

        if (isPoweredUp)
        {
            powerUpTimer += Time.deltaTime;
            if (powerUpTimer >= powerUpDuration)
            {
                EndPowerUp();
            }
        }
    }

    private void HandleCombat()
    {
        agent.speed = chaseSpeed;

        if (Vector3.Distance(player.transform.position, transform.position) <= attackRange)
        {
            if (!isAttacking && !isTakingDamage)
            {
                PerformRandomAttack();
            }
        }
        timePassed += Time.deltaTime;

        if (timePassed >= attackCD && Vector3.Distance(player.transform.position, transform.position) <= attackRange)
        {
            if (!isTakingDamage)
            {
                PerformRandomAttack();
            }
        }

        if (!isAttacking && Vector3.Distance(player.transform.position, transform.position) <= aggroRange)
        {
            agent.SetDestination(player.transform.position);
            animator.SetBool("Chase", true);
        }
    }

    private void PerformRandomAttack()
    {
        int attackIndex = Random.Range(0, attackSkills.Length);
        string randomAttackAnimation = attackSkills[attackIndex];
        animator.SetTrigger(randomAttackAnimation);
        timePassed = 0;
        isAttacking = true;
        agent.isStopped = true;
        animator.SetBool("Chase", false);
        StartCoroutine(TriggerVFXAfterAnimation(attackIndex));
    }

    private IEnumerator TriggerVFXAfterAnimation(int attackIndex)
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        if (isAttacking)
        {
            SpawnSlashVFX(attackIndex);
            EndDealDamage();
        }
    }

    private void HandlePatrol()
    {
        agent.speed = patrolSpeed;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            patrolTimer += Time.deltaTime;

            if (patrolTimer >= patrolWaitTime)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
                agent.SetDestination(patrolPoints[currentPatrolIndex].position);
                patrolTimer = 0;
            }
        }
        else
        {
            patrolTimer = 0;
        }
    }

    private void HandlePlayerDeath()
    {
        StopCoroutine(ReturnToPatrolPoints());
        patrolTimer += Time.deltaTime;
        HandlePatrol();
    }

    private IEnumerator ReturnToPatrolPoints()
    {
        yield return new WaitForSeconds(2f);
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        patrolWaitTime = 3f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject;
        }
    }

    void Die()
    {
        Instantiate(ragDoll, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        animator.SetTrigger("TakeDamage");

        if (health <= 0)
        {
            Die();
        }

        damageCounter++;
        CheckPowerUp();

        isTakingDamage = true;
        StartCoroutine(ResetTakingDamage());
    }

    private IEnumerator ResetTakingDamage()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        isTakingDamage = false;
    }

    void CheckPowerUp()
    {
        if (!isPoweredUp && damageCounter >= damageThreshold)
        {
            isPoweredUp = true;
            bossSlash.DisableSlashes();
            chaseSpeed *= (1 + speedIncreasePercent / 100);
            damageDealer.damage *= (1 + damageIncreasePercent / 100);
            animator.SetTrigger("PowerUp");
            agent.isStopped = true;
            bossSlash.PowerUp();
            powerUpTimer = 0;
        }
    }

    public void EndPowerUp()
    {
        isPoweredUp = false;
        bossSlash.isVFXActive = true;
        chaseSpeed = baseChaseSpeed;
        damageDealer.damage = baseDamage;
        powerUpTimer = 0;
        agent.isStopped = false;
        damageCounter = 0;
    }

    public void StartDealDamage()
    {
        if (damageDealer != null)
        {
            damageDealer.StartDealDamage();
        }
    }

    public void EndDealDamage()
    {
        if (damageDealer != null)
        {
            damageDealer.EndDealDamage();
        }

        isAttacking = false;
        agent.isStopped = false;
    }

    public void SpawnSlashVFX(int attackIndex)
    {
        if (bossSlash != null)
        {
            bossSlash.ActivateSlashes(attackIndex);
        }
    }

    public void HitVFX(Vector3 hitPosition)
    {
        GameObject hit = Instantiate(hitVFX, hitPosition, Quaternion.identity);
        Destroy(hit, 3f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
}