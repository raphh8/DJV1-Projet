using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour, IDamageable
{
    public AbstractState currentState;
    public IdleState Idle = new IdleState();
    public WalkingState Walking = new WalkingState();
    public RunningState Running = new RunningState();
    public JumpState Jump = new JumpState();
    public ShootingState Shoot = new ShootingState();

    public Animator animator;

    public float moveSpeed = 5f;
    public float runSpeedMultiplier = 1.5f;
    public float rollSpeed = 4f;
    public float airSpeed = 1.5f;

    public Vector3 dir;
    CharacterController controller;

    public float horizontal, vertical;

    [SerializeField] float groundYOffset;
    [SerializeField] LayerMask groundMask;
    Vector3 spherePos;
    [SerializeField] float gravity = -15f;
    [SerializeField] float jumpForce = 6;
    public bool jumped;
    public bool shooted;
    Vector3 velocity;

    [SerializeField] public waterBullet waterBulletPrefab;
    [SerializeField] private Transform shootPoint;

    [SerializeField] private int stressLvl = 0;
    [SerializeField] private int maxStress = 100;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        SwitchState(Idle);
    }

    void Update()
    {
        GetDirectionMove();
        Gravity();
        Falling();

        animator.SetFloat("horizontal", horizontal);
        animator.SetFloat("vertical", vertical);

        currentState.UpdateState(this);
    }

    public void SwitchState(AbstractState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
    void GetDirectionMove()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        Vector3 airDir = Vector3.zero;
        if(!IsOnGround()) airDir = transform.forward * vertical + transform.right * horizontal;
        else dir = transform.forward * vertical + transform.right * horizontal;


        float speed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * runSpeedMultiplier : moveSpeed;
        controller.Move((dir.normalized * speed + airDir.normalized * airSpeed) * Time.deltaTime);
    }

    public bool IsOnGround()
    {
        return Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundYOffset, groundMask);
    }

    void Gravity()
    {
        if (!IsOnGround()) velocity.y += gravity * Time.deltaTime;
        else if (velocity.y < 0) velocity.y = -2;

        controller.Move(velocity * Time.deltaTime);
    }

    public void Falling() => animator.SetBool("Falling", !IsOnGround());
    public void JumpForce() => velocity.y += jumpForce;

    public void Jumped() => jumped = true;

    public void Shooted() => shooted = true;

    public void ShootBullet()
    {
        Vector3 shootDirection = transform.forward;
        Vector3 spawnPosition = shootPoint.position;
        Quaternion spawnRotation = Quaternion.LookRotation(shootDirection);

        var bullet = Instantiate(waterBulletPrefab, spawnPosition, spawnRotation);
        bullet.gameObject.SetActive(true);
    }



    public void ApplyDamage(int value)
    {
        stressLvl += value;
        stressLvl = Mathf.Clamp(stressLvl, 0, maxStress);

        if (stressLvl >= maxStress)
        {
            Destroy(gameObject);
        }
    }

    public float StressPercent => (float)stressLvl / maxStress;
}



