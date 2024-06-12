using UnityEngine;

public class CombatState : State
{
    float gravityValue;
    Vector3 currentVelocity;
    bool grounded;
    bool sheathWeapon;
    float playerSpeed;
    bool attack;
    bool roll;

    Vector3 cVelocity;

    public CombatState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        sheathWeapon = false;
        roll = false;
        input = Vector2.zero;
        currentVelocity = Vector3.zero;
        gravityVelocity.y = 0;
        attack = false;

        velocity = character.playerVelocity;
        playerSpeed = character.playerSpeed;
        grounded = character.controller.isGrounded;
        gravityValue = character.gravityValue;
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (EquipmentSystem.Instance.IsWeaponEquipped())
        {
            if (drawWeaponAction.triggered)
            {
                sheathWeapon = true;
            }
        }

        if (EquipmentSystem.Instance.CanAttack())
        {
            if (attackAction.triggered)
            {
                attack = true;
            } 
        }

        if (rollAction.triggered && character.combatRolling.CanRoll())
        {
            roll = true;
        }

        input = moveAction.ReadValue<Vector2>();
        velocity = new Vector3(input.x, 0, input.y);

        velocity = velocity.x * character.cameraTransform.right.normalized + velocity.z * character.cameraTransform.forward.normalized;
        velocity.y = 0f;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        character.animator.SetFloat("Speed", input.magnitude, character.speedDampTime, Time.deltaTime);

        if (sheathWeapon && EquipmentSystem.Instance.IsWeaponEquipped())
        {
            character.animator.SetTrigger("SheathWeapon");
            stateMachine.ChangeState(character.standing);
        }

        if (attack && EquipmentSystem.Instance.CanAttack())
        {
            character.animator.SetTrigger("Attack1");
            stateMachine.ChangeState(character.attacking);
        }

        if (roll)
        {
            character.animator.SetTrigger("Roll");
            stateMachine.ChangeState(character.combatRolling);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        gravityVelocity.y += gravityValue * Time.deltaTime;
        grounded = character.controller.isGrounded;

        if (grounded && gravityVelocity.y < 0)
        {
            gravityVelocity.y = 0f;
        }

        currentVelocity = Vector3.SmoothDamp(currentVelocity, velocity, ref cVelocity, character.velocityDampTime);
        character.controller.Move(playerSpeed * Time.deltaTime * currentVelocity + gravityVelocity * Time.deltaTime);

        if (velocity.sqrMagnitude > 0)
        {
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(velocity), character.rotationDampTime);
        }
    }

    public override void Exit()
    {
        base.Exit();

        gravityVelocity.y = 0f;
        character.playerVelocity = new Vector3(input.x, 0, input.y);

        if (velocity.sqrMagnitude > 0)
        {
            character.transform.rotation = Quaternion.LookRotation(velocity);
        }
    }
}