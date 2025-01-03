using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    AbstractState currentState;
    public IdleState Idle = new IdleState();
    public WalkingState Walking = new WalkingState();
    public RunningState Running = new RunningState();
    public Animator animator;

    public float moveSpeed = 3f;
    public float runSpeedMultiplier = 1.5f;
    public float rollSpeed = 6f;

    public Vector3 dir;
    CharacterController controller;

    float horizontal, vertical;

    [SerializeField] float groundYOffset;
    [SerializeField] LayerMask groundMask;
    Vector3 spherePos;
    [SerializeField] float gravity = -9.81f;
    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        SwitchState(Idle);
    }

    // Update is called once per frame
    void Update()
    {
        GetDirectionMove();
        Gravity();

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

        float speed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * runSpeedMultiplier : moveSpeed;
        dir = transform.forward * vertical + transform.right * horizontal;
        controller.Move(dir.normalized * speed * Time.deltaTime);
    }

    bool IsOnGround()
    {
        spherePos = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
        return Physics.CheckSphere(spherePos, controller.radius - 0.05f, groundMask);
    }

    void Gravity()
    {
        if (!IsOnGround()) velocity.y += gravity * Time.deltaTime;
        else if (velocity.y < 0) velocity.y = -2;

        controller.Move(velocity * Time.deltaTime);
    }
}



