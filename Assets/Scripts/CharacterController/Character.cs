using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    [Header("Controls")]
    public float playerSpeed = 5.0f;
    public float crouchSpeed = 2.0f;
    public float sprintSpeed = 8.0f;
    public float jumpHeight = 0.8f;
    public float gravityMultiplier = 2;
    public float rotationSpeed = 5f;
    public float crouchColliderHeight = 1.35f;

    [Header("Animation Smoothing")]
    [Range(0, 1)]
    public float speedDampTime = 0.1f;
    [Range(0, 1)]
    public float velocityDampTime = 0.9f;
    [Range(0, 1)]
    public float rotationDampTime = 0.2f;
    [Range(0, 1)]
    public float airControl = 0.5f;

    [Header("Combo")]
    public int maxComboCount = 3;
    private int currentComboCount = 0;
    private float comboTimer = 0f;
    public float comboWindow = 1.0f;

    [Header("GroundSlash Effect")]
    public GameObject groundSlashPrefab;
    public float groundSlashSpeed = 10f;

    public StateMachine movementSM;
    public StandingState standing;
    public JumpingState jumping;
    public CrouchingState crouching;
    public LandingState landing;
    public SprintState sprinting;
    public SprintJumpState sprintJumping;
    public CombatState combatting;
    public AttackState attacking;
    public RollState rolling;
    public CombatRollState combatRolling;

    [HideInInspector] public float gravityValue = -9.81f;
    [HideInInspector] public float normalColliderHeight;
    [HideInInspector] public CharacterController controller;
    [HideInInspector] public Slash slash;
    [HideInInspector] public PlayerInput playerInput;
    [HideInInspector] public Transform cameraTransform;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Vector3 playerVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        slash = GetComponent<Slash>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;

        movementSM = new StateMachine();
        standing = new StandingState(this, movementSM);
        jumping = new JumpingState(this, movementSM);
        crouching = new CrouchingState(this, movementSM);
        landing = new LandingState(this, movementSM);
        sprinting = new SprintState(this, movementSM);
        sprintJumping = new SprintJumpState(this, movementSM);
        combatting = new CombatState(this, movementSM);
        attacking = new AttackState(this, movementSM);
        rolling = new RollState(this, movementSM);
        combatRolling = new CombatRollState(this, movementSM);

        movementSM.Initialize(standing);

        normalColliderHeight = controller.height;
        gravityValue *= gravityMultiplier;
    }

    void Update()
    {
        if (currentComboCount > 0)
        {
            comboTimer += Time.deltaTime;
            if (comboTimer > comboWindow)
            {
                ResetCombo();
            }
        }

        movementSM.currentState.HandleInput();
        movementSM.currentState.LogicUpdate();
    }

    public bool IsAttackButtonPressed()
    {
        return playerInput.actions["Attack"].triggered;
    }


    public void ResetCombo()
    {
        currentComboCount = 0;
        comboTimer = 0f;
    }

    public void IncrementCombo()
    {
        currentComboCount = Mathf.Clamp(currentComboCount + 1, 0, maxComboCount);
        comboTimer = 0f;
    }

    public int GetCurrentComboCount()
    {
        return currentComboCount;
    }

    public void SpawnGroundSlash()
    {
        if (groundSlashPrefab != null)
        {
            GameObject groundSlash = Instantiate(groundSlashPrefab, transform.position + transform.forward, Quaternion.identity);
            if (groundSlash.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.velocity = transform.forward * groundSlashSpeed;
                groundSlash.transform.rotation = Quaternion.LookRotation(rb.velocity);
            }
        }
    }

    void FixedUpdate()
    {
        movementSM.currentState.PhysicsUpdate();
    }

    public void SpawnSlashVFX(int index)
    {
        slash.ActivateSlash(index);
    }
}