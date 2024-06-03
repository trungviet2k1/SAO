using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BossSlash : MonoBehaviour
{
    public List<BossSlashVFX> vfxBossSlash;
    public VisualEffect powerUp;

    [HideInInspector] public bool isVFXActive = false;
    [HideInInspector] public bool poweringUp = false;

    void Start()
    {
        DisableSlashes();
        powerUp.Stop();
    }

    public void DisableSlashes()
    {
        foreach (var vfx in vfxBossSlash)
        {
            vfx.bossSlashObj.SetActive(false);
        }
        isVFXActive = false;
    }

    public void ActivateSlashes(int attackIndex)
    {
        if (attackIndex >= 0 && attackIndex < vfxBossSlash.Count)
        {
            isVFXActive = true;
            vfxBossSlash[attackIndex].bossSlashObj.SetActive(true);
            StartCoroutine(DisableAfterDelay(vfxBossSlash[attackIndex].bossSlashObj, vfxBossSlash[attackIndex].timeDelay));
        }
    }

    public void PowerUp()
    {
        if (powerUp != null)
        {
            powerUp.Play();
        }

        poweringUp = true;
        StartCoroutine(ResetBool(poweringUp, 0.5f));
    }

    IEnumerator ResetBool(bool boolToReset, float delay = 0.1f)
    {
        yield return new WaitForSeconds(delay);
        poweringUp = !poweringUp;
    }

    IEnumerator DisableAfterDelay(GameObject slashObj, float delay)
    {
        yield return new WaitForSeconds(delay);
        slashObj.SetActive(false);
        isVFXActive = false;
    }
}

[System.Serializable]
public class BossSlashVFX
{
    public GameObject bossSlashObj;
    public float timeDelay;
}