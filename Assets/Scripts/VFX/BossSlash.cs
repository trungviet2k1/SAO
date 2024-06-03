using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlash : MonoBehaviour
{
    public List<BossSlashVFX> vfxBossSlash;
    private bool isVFXActive = false;

    void Start()
    {
        DisableSlashes();
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
            if (!isVFXActive)
            {
                isVFXActive = true;
                vfxBossSlash[attackIndex].bossSlashObj.SetActive(true);
                StartCoroutine(DisableAfterDelay(vfxBossSlash[attackIndex].bossSlashObj, vfxBossSlash[attackIndex].timeDelay));
            }
        }
    }

    private IEnumerator DisableAfterDelay(GameObject slashObj, float delay)
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