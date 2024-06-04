using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public List<SlashVFX> vfxSlash;

    void Start()
    {
        DisableSlashes();
    }

    public void DisableSlashes()
    {
        foreach (var slash in vfxSlash)
        {
            slash.slashObj.SetActive(false);
        }
    }

    public void ActivateSlash(int index)
    {
        if (index >= 0 && index < vfxSlash.Count)
        {
            StartCoroutine(ActivateSingleSlash(vfxSlash[index]));
        }
    }

    private IEnumerator ActivateSingleSlash(SlashVFX slash)
    {
        yield return new WaitForSeconds(slash.delay);
        slash.slashObj.SetActive(true);

        yield return new WaitForSeconds(1);
        slash.slashObj.SetActive(false);
    }
}

[System.Serializable]
public class SlashVFX
{
    public GameObject slashObj;
    public float delay;
}