using UnityEngine;

public class Fighter : CharacterBase
{
    public int strength;

    public override void Attack()
    {
        base.Attack();
        Debug.Log(characterName + " punches the enemy!");
    }

    public override void UseSkill()
    {
        Debug.Log(characterName + " uses Power Punch!");
    }
}