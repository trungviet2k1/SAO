using UnityEngine;

public class CombatRollState : State
{
    private float rollDuration = 0.5f;
    private float rollSpeed = 10.0f;
    private float elapsedTime = 0.0f;
    public float cooldownTime = 3.0f;
    public float lastRollTime = -Mathf.Infinity;
    private Vector3 rollDirection;

    public CombatRollState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        elapsedTime = 0.0f;
        character.animator.SetTrigger("Roll");

        input = moveAction.ReadValue<Vector2>();
        if (input != Vector2.zero)
        {
            rollDirection = new Vector3(input.x, 0, input.y).normalized;
            rollDirection = character.cameraTransform.TransformDirection(rollDirection);
            rollDirection.y = 0;
            rollSpeed = 10f;
        }
        else
        {
            rollDirection = character.transform.forward;
            rollSpeed = 5f;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= rollDuration)
        {
            character.animator.SetTrigger("Move");
            stateMachine.ChangeState(character.combatting);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        character.controller.Move(rollSpeed * Time.deltaTime * rollDirection);
    }

    public override void Exit()
    {
        base.Exit();
        lastRollTime = Time.time;
    }

    public bool CanRoll()
    {
        return Time.time >= lastRollTime + cooldownTime;
    }
}