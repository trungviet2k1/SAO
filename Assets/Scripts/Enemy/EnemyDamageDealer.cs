using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour
{
    bool canDealDamage;
    bool hasDealtDamage;

    [SerializeField] float weaponLength;
    [SerializeField] float weaponDamage;

    void Start()
    {
        canDealDamage = false;
        hasDealtDamage = false;
    }

    void Update()
    {
        if (canDealDamage && !hasDealtDamage)
        {
            int layerMask = 1 << 8;
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, weaponLength, layerMask))
            {
                if (hit.transform.TryGetComponent(out HealthSystem health))
                {
                    health.TakeDamage(weaponDamage);
                    health.HitVFX(hit.point);
                    hasDealtDamage = true;
                }
            }
        }
    }

    public void StartDealDamage()
    {
        canDealDamage = true;
        hasDealtDamage = false;
    }

    public void EndDealDamage()
    {
        canDealDamage = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * weaponLength);
    }
}