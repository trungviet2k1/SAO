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

    public void ActivateSlashes()
    {
        StartCoroutine(SlashAttacks());
    }

    private IEnumerator SlashAttacks()
    {
        foreach (var slash in vfxSlash)
        {
            yield return new WaitForSeconds(slash.delay);
            slash.slashObj.SetActive(true);
        }

        yield return new WaitForSeconds(1);
        DisableSlashes();
    }
}

[System.Serializable]
public class SlashVFX
{
    public GameObject slashObj;
    public float delay;
}