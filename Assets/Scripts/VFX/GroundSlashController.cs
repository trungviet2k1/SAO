using UnityEngine;

public class GroundSlashController : MonoBehaviour
{
    public float damage;
    public float lifeTime;
    private Rigidbody rb;

    void Start()
    {
        Destroy(gameObject, lifeTime);
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (rb.velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(damage);
            }

            if (other.TryGetComponent(out BossHealthSystem boss))
            {
                boss.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}