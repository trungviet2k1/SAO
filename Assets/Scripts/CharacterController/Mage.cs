using UnityEngine;

public class Mage : CharacterBase
{
    public int magicPower;

    public override void Attack()
    {
        base.Attack();
        Debug.Log(characterName + " casts a spell!");
    }

    public override void UseSkill()
    {
        Debug.Log(characterName + " uses Fireball!");
    }
}