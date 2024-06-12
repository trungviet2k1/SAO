using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Character playerCharacter;

    void Start()
    {
        playerCharacter = FindObjectOfType<Character>();
    }

    public void AwardXP(int amount)
    {
        playerCharacter.GainExperience(amount);
    }
}