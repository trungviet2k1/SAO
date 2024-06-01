using UnityEngine;

public class Archer : CharacterBase
{
    public int bowSkill;

    public override void Attack()
    {
        base.Attack();
        Debug.Log(characterName + " shoots an arrow!");
    }

    public override void UseSkill()
    {
        Debug.Log(characterName + " uses Explosive Arrow!");
    }
}