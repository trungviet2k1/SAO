using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    public string characterName;
    public int level;
    public int health;
    public int mana;

    public virtual void Attack()
    {
        Debug.Log(characterName + " attacks!");
    }

    public virtual void UseSkill()
    {
        Debug.Log(characterName + " uses a skill!");
    }
}