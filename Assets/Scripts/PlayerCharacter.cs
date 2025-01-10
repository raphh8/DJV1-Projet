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
    public RollState Roll = new RollState();

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
    public bool rolled;
    Vector3 velocity;

    [SerializeField] public waterBullet waterBulletPrefab;
    [SerializeField] private Transform shootPoint;

    private int stressLvl;
    [SerializeField] private int maxStress = 100;

    [SerializeField] private GameObject gameOverPanel;

    private bool isDead;
    private int enemiesKilled = 0;
    public bool bonus1, bonus2;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        gameOverPanel.SetActive(false);
        SwitchState(Idle);

        stressLvl = 0;
        isDead = false;
        bonus1 = false;
        bonus2 = true;
    }

    void Update()
    {
        if (isDead) return;

        GetDirectionMove();
        Gravity();
        Falling();

        animator.SetFloat("horizontal", horizontal);
        animator.SetFloat("vertical", vertical);

        currentState.UpdateState(this);
    }

    public void UpgradeStats()
    {
        moveSpeed += 0.5f;
        runSpeedMultiplier += 0.1f;
        rollSpeed += 0.3f;
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


        float speed = moveSpeed;
        if(Input.GetKey(KeyCode.LeftShift) && bonus1) speed *= runSpeedMultiplier;
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

    public void OnEnemyKilled()
    {
        enemiesKilled++;

        if (enemiesKilled >= 3 && !bonus1)
        {
            bonus1 = true;
            Debug.Log("Bonus 1 débloqué : Vous pouvez courir !");
        }

        if (enemiesKilled >= 1 && !bonus2)
        {
            bonus2 = true;
            Debug.Log("Bonus 2 débloqué : Vous pouvez faire une roulade !");
        }
    }

    public void Falling() => animator.SetBool("Falling", !IsOnGround());

    public void JumpForce() => velocity.y += jumpForce;

    public void Jumped() => jumped = true;

    public void Shooted() => shooted = true;

    public void Rolled() => rolled = true;

    public void ShootBullet()
    {
        Vector3 shootDirection = transform.forward;
        Vector3 spawnPosition = shootPoint.position;
        Quaternion spawnRotation = Quaternion.LookRotation(shootDirection);

        var bullet = Instantiate(waterBulletPrefab, spawnPosition, spawnRotation);
        bullet.gameObject.SetActive(true);
    }

    public void RollPerform()
    {
        Vector3 rollDirection = dir.normalized;
        Vector3 rollMovement = rollDirection * rollSpeed * Time.deltaTime;
        controller.Move(rollMovement);
    }

    public void ApplyDamage(int value)
    {
        if (isDead) return;

        stressLvl += value;
        stressLvl = Mathf.Clamp(stressLvl, 0, maxStress);

        if (stressLvl >= maxStress)
        {
            foreach (EnemyCharacter enemy in FindObjectsOfType<EnemyCharacter>())
            {
                enemy.NotifyPlayerDead();
            }
            isDead = true;
            gameOverPanel.SetActive(true);
            controller.enabled = false;
            Time.timeScale = 0f;
            Destroy(gameObject);
        }
    }

    public float StressPercent => (float)stressLvl / maxStress;
}



