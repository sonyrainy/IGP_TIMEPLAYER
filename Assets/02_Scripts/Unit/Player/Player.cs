using Player_States;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerState prevPlayerState = PlayerState.Idle;
    public PlayerState playerState = PlayerState.Idle;

    public Animator animator;
    public Transform transform;
    public Rigidbody2D rigidbody;

    [SerializeField] LayerMask floorLayer;
    [SerializeField] public float moveSpeed = 0;
    [SerializeField] public float dashFloat = 0;
    [SerializeField] public float jumpForce = 1f;

    [SerializeField] float castSize;
    [SerializeField] float gravity = 9.8f;
    [SerializeField] public float yVelocity = 0;

    [SerializeField] public bool isGround = false;
    public bool isDash = false;

    public State<Player>[] states;

    public float speedMultiplier = 1f;
    public bool isInTimeZone = false; 
    public float inTimeZoneSpeed = 1;

    public void ChangeState(PlayerState state)
    {
        states[(int)playerState].Exit();
        prevPlayerState = playerState;
        playerState = state;
        states[(int)playerState].Enter();
    }

    public void ChangeAnimation(PlayerAnimation newAnimation, float normalizedTime = 0)
    {
        animator.Play(newAnimation.ToString(), 0, normalizedTime);
    }

    public void ChangeAnimation(string animationName, float normalizedTime = 0)
    {
        animator.Play(animationName, 0, normalizedTime);
    }

    void Start()
    {
        animator =  GetComponentInChildren<Animator>();
        transform = GetComponentInChildren<Transform>();
        rigidbody = GetComponentInChildren<Rigidbody2D>();
    }

    private void OnEnable()
    {
        InitFSM();
    }

    // Update is called once per frame
    void Update()
    {
        // 타임존에 들어가 있으면 애니메이션 속도 감속 및 가속
        if (isInTimeZone)
        {
            animator.speed = inTimeZoneSpeed;
        }
        else
        {
            animator.speed = 1.0f;
        }

        GroundCheck();
        if (!isDash)
        {
            ApplyGravity();
        }

        states[(int)playerState].Execute();
        states[(int)playerState].OnTransition();

        if (!isGround)
        {
            animator.SetFloat("YSpeed", yVelocity);
        }
    }

    void InitFSM()
    {
        animator = GetComponentInChildren<Animator>();
        playerState = PlayerState.Spawn;

        states = new State<Player>[(int)PlayerState.Last];

        states[(int)PlayerState.Idle] = new Idle(this);
        states[(int)PlayerState.Spawn] = new Spawn(this);
        states[(int)PlayerState.Run] = new Run(this);
        states[(int)PlayerState.Jump] = new Jump(this);
        states[(int)PlayerState.Dash] = new Dash(this);
        states[(int)PlayerState.Hit] = new Hit(this);

        states[(int)playerState].Enter();
    }

    // 플레이어가 바닥에 붙어 있는지 확인
    void GroundCheck()
    {
        if (yVelocity <= 0)
        {
            Debug.DrawLine(rigidbody.position + Vector2.up, rigidbody.position + Vector2.up + (Vector2.down* castSize), Color.red);

            RaycastHit2D rayHit = Physics2D.Raycast(rigidbody.position + Vector2.up, Vector3.down, castSize, floorLayer);
            if (rayHit.collider != null)
            {
                if (!isGround)
                {
                    animator.SetTrigger("OnGround");
                    transform.position = rayHit.point;
                }
                isGround = true;
                yVelocity = 0;
            }
            else
            {
                isGround = false;
            }
        }
    }

    public void AdjustObjectSpeed(float speedMultiplier)
    {
        this.speedMultiplier *= speedMultiplier;

        // �̵� �ӵ� ����
        moveSpeed *= speedMultiplier;

        // �ִϸ����� �ӵ� ����
        inTimeZoneSpeed *= speedMultiplier;
        animator.speed = inTimeZoneSpeed;
    }

    // 플레이어 중력 적용
    public void ApplyGravity()
    {
        if (!isGround)
        {
            yVelocity -= gravity * gravity * Time.deltaTime * speedMultiplier;
            yVelocity = Mathf.Max(yVelocity, -20);
        }
        Vector3 position = transform.position;

        // yVelocity * Time.deltaTime * speedMultiplier 
        // yVelocity * Time.deltaTime                   
        position.y += yVelocity * Time.deltaTime * speedMultiplier;
        transform.position = position;
    }

    public void OnDie()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BossAttackObjects"))
        {
            ChangeState(PlayerState.Hit);
        }
    }
}
