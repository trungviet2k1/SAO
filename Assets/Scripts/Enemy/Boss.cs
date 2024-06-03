using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField] float attackCD;
    [SerializeField] float attackRange;
    [SerializeField] float aggroRange;
    [SerializeField] string[] attackSkills;

    [Header("Patrol Settings")]
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] float patrolSpeed;
    [SerializeField] float chaseSpeed;
    [SerializeField] float patrolWaitTime;

    [Header("Power Up")]
    [SerializeField] float damageIncreasePercent;
    [SerializeField] float speedIncreasePercent;
    [SerializeField] int damageThreshold = 5;

    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isTakingDamage;
    [HideInInspector] public int damageCounter = 0;

    GameObject player;
    Animator animator;
    NavMeshAgent agent;
    BossDamageDealer damageDealer;
    BossSlash bossSlash;
    float timePassed;
    float powerUpTimer;
    int currentPatrolIndex;
    bool playerDetected;
    float patrolTimer;
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

    public void CheckPowerUp()
    {
        if (!isPoweredUp && damageCounter >= damageThreshold)
        {
            isPoweredUp = true;
            isAttacking = false;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
}