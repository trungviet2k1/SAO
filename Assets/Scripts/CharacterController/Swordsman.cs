using UnityEngine;

public class Swordsman : CharacterBase
{
    public int swordSkill;

    public override void Attack()
    {
        base.Attack();
        Debug.Log(characterName + " swings sword!");
    }

    public override void UseSkill()
    {
        Debug.Log(characterName + " uses Whirlwind Slash!");
    }
}