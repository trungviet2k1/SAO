using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public static HealthSystem Instance;

    [Header("Player Health Setting")]
    [SerializeField] public float maxHealth;
    [HideInInspector] public float currentHealth;

    [Header("UI")]
    [SerializeField] Slider healthSlider;
    [SerializeField] TextMeshProUGUI healthValue;

    [Header("Effect and Ragdoll")]
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject ragdoll;

    public delegate void HealthChangedDelegate(float currentHealth);
    public event HealthChangedDelegate OnHealthChanged;

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
        UpdateHealthValue();
    }

    void Update()
    {
        if (healthSlider.value != currentHealth)
        {
            UpdateHealthValue();
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        animator.SetTrigger("TakeDamage");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            UpdateHealthValue();
            Die();
        }
        else
        {
            UpdateHealthValue();
        }
    }

    public void RestoreHealth(float healthAmount)
    {
        currentHealth += healthAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateHealthValue();
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

    void UpdateHealthValue()
    {
        healthValue.text = $"HP: {currentHealth}";
        healthSlider.value = currentHealth;

        OnHealthChanged?.Invoke(currentHealth);
    }
}