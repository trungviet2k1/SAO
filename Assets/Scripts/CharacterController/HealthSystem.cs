using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject ragdoll;

    Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        animator.SetTrigger("TakeDamage");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(ragdoll, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public void HitVFX(Vector3 hitPosition)
    {
        GameObject hit = Instantiate(hitVFX, hitPosition, Quaternion.identity);
        Destroy(hit, 3f);
    }
}