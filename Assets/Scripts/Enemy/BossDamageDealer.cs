using UnityEngine;

public class BossDamageDealer : MonoBehaviour
{
    public float damage;
    bool isDealingDamage = false;

    void OnTriggerEnter(Collider other)
    {
        if (isDealingDamage && other.CompareTag("Player"))
        {
            if (other.TryGetComponent<HealthSystem>(out var playerHealth))
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    public void StartDealDamage()
    {
        isDealingDamage = true;
    }

    public void EndDealDamage()
    {
        isDealingDamage = false;
    }
}