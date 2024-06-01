using UnityEngine;

public class ShockwaveSkills : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 10f;

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.forward);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealthSystem playerHealth = other.GetComponent<HealthSystem>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Enemy"))
        {
            // Hủy đạn khi va chạm với bất kỳ vật thể nào khác ngoài boss
            Destroy(gameObject);
        }
    }
}