using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    bool canDealDamage;
    List<GameObject> hasDealDamage;

    [SerializeField] float weaponLength;
    [SerializeField] WeaponItem weaponAttackPower;
    public float weaponDamage = 0;

    void Start()
    {
        canDealDamage = false;
        hasDealDamage = new List<GameObject>();

        if (weaponAttackPower != null)
        {
            weaponDamage = weaponAttackPower.attackPower;
        }
    }

    void Update()
    {
        if (canDealDamage)
        {
            int layerMask = 1 << 9;
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, weaponLength, layerMask))
            {
                if (hit.transform.TryGetComponent(out Enemy enemy) && !hasDealDamage.Contains(hit.transform.gameObject))
                {
                    enemy.TakeDamage(weaponDamage);
                    enemy.HitVFX(hit.point);
                    hasDealDamage.Add(hit.transform.gameObject);
                }

                if (hit.transform.TryGetComponent(out BossHealthSystem bossHealth) && !hasDealDamage.Contains(hit.transform.gameObject))
                {
                    bossHealth.TakeDamage(weaponDamage);
                    bossHealth.HitVFX(hit.point);
                    hasDealDamage.Add(hit.transform.gameObject);
                }
            }
        }
    }

    public void StartDealDamage()
    {
        canDealDamage = true;
        hasDealDamage.Clear();
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