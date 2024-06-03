using System.Collections;
using UnityEngine;

public class BossHealthSystem : MonoBehaviour
{
    [Header("Boss Health Setting")]
    [SerializeField] float maxHealth;
    [HideInInspector] public float currentHealth;

    [Header("Effect and Ragdoll")]
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject ragDoll;

    Animator animator;
    Boss boss;


    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        animator = GetComponent<Animator>();
        boss = GetComponent<Boss>();
    }

    public void TakeDamage(float damageAmount)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("PowerUp"))
        {
            return;
        }

        maxHealth -= damageAmount;
        animator.SetTrigger("TakeDamage");

        if (maxHealth <= 0)
        {
            Die();
        }

        boss.damageCounter++;
        boss.CheckPowerUp();

        boss.isTakingDamage = true;
        boss.isAttacking = false;
        StartCoroutine(ResetTakingDamage());
    }

    private IEnumerator ResetTakingDamage()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        boss.isTakingDamage = false;
    }

    void Die()
    {
        Instantiate(ragDoll, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public void HitVFX(Vector3 hitPosition)
    {
        GameObject hit = Instantiate(hitVFX, hitPosition, Quaternion.identity);
        Destroy(hit, 3f);
    }
}