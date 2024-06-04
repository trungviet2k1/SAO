using UnityEngine;

public class AttackState : State
{
    float timePassed;
    float clipLength;
    float clipSpeed;
    bool attack;

    public AttackState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        attack = false;
        character.animator.applyRootMotion = true;
        timePassed = 0f;

        character.IncrementCombo();
        int comboStep = character.GetCurrentComboCount();

        character.animator.SetTrigger("Attack" + comboStep);
        character.animator.SetFloat("Speed", 0f);

        character.SpawnSlashVFX(comboStep - 1);
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (attackAction.triggered)
        {
            attack = true;
        }

        /*if (!attackAction.IsPressed())
        {
            attack = false;
        }*/
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        timePassed += Time.deltaTime;
        clipLength = character.animator.GetCurrentAnimatorClipInfo(1)[0].clip.length;
        clipSpeed = character.animator.GetCurrentAnimatorStateInfo(1).speed;

        if (timePassed >= clipLength / clipSpeed && attack)
        {
            stateMachine.ChangeState(character.attacking);
        }
        if (timePassed >= clipLength / clipSpeed)
        {
            character.ResetCombo();
            stateMachine.ChangeState(character.combatting);
            character.animator.SetTrigger("Move");
        }
    }

    public override void Exit()
    {
        base.Exit();
        character.animator.applyRootMotion = false;
    }
}