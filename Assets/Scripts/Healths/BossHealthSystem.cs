using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthSystem : MonoBehaviour
{
    [Header("Boss Health Setting")]
    [SerializeField] float maxHealth;
    [HideInInspector] public float currentHealth;

    [Header("UI")]
    [SerializeField] GameObject bossHealthFrameUI;
    [SerializeField] Slider healthSlider;
    [SerializeField] TextMeshProUGUI healthIndex;

    [Header("Effect and Ragdoll")]
    [SerializeField] GameObject hitVFX;
    [SerializeField] GameObject ragDoll;

    Animator animator;
    Boss boss;
    Coroutine healthRegenCoroutine;

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        animator = GetComponent<Animator>();
        boss = GetComponent<Boss>();
        bossHealthFrameUI.SetActive(false);
        UpdateHealthIndex();
    }

    void Update()
    {
        if (healthSlider.value != currentHealth)
        {
            UpdateHealthIndex();
        }
    }

    void UpdateHealthIndex()
    {
        healthIndex.text = $"{currentHealth} / {maxHealth}";
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("PowerUp"))
        {
            return;
        }

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

    public void InRangeOfBoss()
    {
        bossHealthFrameUI.SetActive(true);
        StopHealthRegen();
    }

    public void OutRangeOfBoss()
    {
        bossHealthFrameUI.SetActive(false);
        StartHealthRegen();
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthIndex();
    }

    private void StartHealthRegen()
    {
        if (healthRegenCoroutine == null)
        {
            healthRegenCoroutine = StartCoroutine(RegenerateHealth());
        }
    }

    private void StopHealthRegen()
    {
        if (healthRegenCoroutine != null)
        {
            StopCoroutine(healthRegenCoroutine);
            healthRegenCoroutine = null;
        }
    }

    private IEnumerator RegenerateHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            if (currentHealth < maxHealth)
            {
                currentHealth += 1;
                UpdateHealthIndex();
            }
        }
    }
}