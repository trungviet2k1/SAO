using UnityEngine;

public class SprintJumpState : State
{
    float timePassed;
    float jumpTime;
    private Vector3 jumpDirection;

    public SprintJumpState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        character.animator.applyRootMotion = false;
        timePassed = 0.5f;
        character.animator.SetTrigger("SprintJump");
        jumpTime = 1f;

        jumpDirection = character.transform.forward;
        jumpDirection.y = 0;
        jumpDirection.Normalize();
        jumpDirection *= character.sprintSpeed;

        character.playerVelocity = new Vector3(jumpDirection.x, CalculateJumpVerticalSpeed(character.jumpHeight * 1.5f), jumpDirection.z);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (timePassed > jumpTime)
        {
            character.animator.SetTrigger("Move");
            stateMachine.ChangeState(character.sprinting);
        }

        timePassed += Time.deltaTime;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (!character.controller.isGrounded)
        {
            character.playerVelocity.y += character.gravityValue * Time.deltaTime;
        }

        character.transform.rotation = Quaternion.Euler(0, character.transform.rotation.eulerAngles.y, 0);
        character.controller.Move(character.playerVelocity * Time.deltaTime);
    }

    private float CalculateJumpVerticalSpeed(float jumpHeight)
    {
        return Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(character.gravityValue));
    }
}