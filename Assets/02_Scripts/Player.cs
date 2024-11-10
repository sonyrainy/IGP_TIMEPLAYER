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
    [SerializeField] public float dashSpeed = 0;
    [SerializeField] public float jumpForce = 1f;

    [SerializeField] float castSize;
    [SerializeField] float gravity = 9.8f;
    [SerializeField] public float yVelocity = 0;

    [SerializeField] public bool isGround = false;

    public State<Player>[] states;

    public float speedMultiplier = 1f;
    public bool isInTimeZone = false; 
    public float animationSpeed = 1;

    public void ChangeState(PlayerState state)
    {
        states[(int)playerState].Exit();
        prevPlayerState = playerState;
        playerState = state;
        states[(int)playerState].Enter();
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
        // 타임존에 있는 경우 애니메이터 속도 조절
        if (isInTimeZone)
        {
            animator.speed = animationSpeed;
        }
        else
        {
            animator.speed = 1.0f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeState(PlayerState.Jump);
        }

        GroundCheck();
        ApplyGravity();

        states[(int)playerState].Execute();
        states[(int)playerState].OnTransition();

        if (!isGround)
        {
            animator.SetFloat("YSpeed", yVelocity);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {

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

        states[(int)playerState].Enter();
    }

    // 플레이어가 지면과 닿아 있는지 확인
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

        // 이동 속도 조정
        moveSpeed *= speedMultiplier;

        // 애니메이터 속도 조정
        animationSpeed *= speedMultiplier;
        animator.speed = animationSpeed;
    }

    // 중력 적용 함수
    private void ApplyGravity()
    {
        if (!isGround)
        {
            yVelocity -= gravity * gravity * Time.deltaTime * speedMultiplier;
            yVelocity = Mathf.Max(yVelocity, -20);
        }
        Vector3 position = transform.position;

        // yVelocity * Time.deltaTime * speedMultiplier => 높이 고정
        // yVelocity * Time.deltaTime                   => 시간 느릴 땐 낮게, 시간 빠를 땐 높게
        position.y += yVelocity * Time.deltaTime * speedMultiplier;
        transform.position = position;
    }

}
