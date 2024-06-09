using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public static HealthSystem Instance;

    [Header ("Player Health Setting")]
    [SerializeField] public float maxHealth;
    [HideInInspector] public float currentHealth;

    [Header ("UI")]
    [SerializeField] Slider healthSlider;
    [SerializeField] TextMeshProUGUI healthIndex;

    [Header ("Effect and Ragdoll")]
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject ragdoll;

    Animator animator;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        animator = GetComponent<Animator>();
        UpdateHealthIndex();
    }

    void Update()
    {
        if (healthSlider.value != currentHealth)
        {
            UpdateHealthIndex();
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        animator.SetTrigger("TakeDamage");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            UpdateHealthIndex();
            Die();
        }
        else
        {
            UpdateHealthIndex();
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

    void UpdateHealthIndex()
    {
        healthIndex.text = $"{currentHealth} / {maxHealth}";
        healthSlider.value = currentHealth;

        if (InventorySystem.Instance != null)
        {
            InventorySystem.Instance.UpdateHealthValue(currentHealth);
        }
    }
}