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
    public float animationSpeed = 1;
    
    //현택이 코드
    //public float inTimeZoneSpeed = 1;
// 추가된 변수들
    public Transform[] spawnPoints; // 스폰 포인트들
    private int lastSpawnPointIndex = 0; // 마지막으로 도달한 스폰 포인트의 인덱스
    
    
    
    public float fallDeathVelocity = -36.0f; // 이 속도 이상으로 떨어질 경우 플레이어가 죽음


    // yVelocity의 최소값 설정
    //private float yVelocityMinDefault = -20f;
    private float yVelocityMin = -20f;
    private float yVelocityMinSlowTimeZone = -10f;
    private float yVelocityMinFastTimeZone = -45f;

    public void ChangeState(PlayerState state)
    {
        states[(int)playerState].Exit();
        prevPlayerState = playerState;
        playerState = state;
        states[(int)playerState].Enter();

        // 점프 상태로 변경될 때 한 번만 점프 소리 재생
        if (state == PlayerState.Jump && prevPlayerState != PlayerState.Jump)
        {
            SoundManager.Instance.PlaySFX("Jump", 0.2f, false);
        }
    }

    public void ChangeAnimation(PlayerAnimation newAnimation, float normalizedTime = 0)
    {
        animator.Play(newAnimation.ToString(), 0, normalizedTime);
    }

    public void ChangeAnimation(string animationName, float normalizedTime = 0)
    {
        animator.Play(animationName, 0, normalizedTime);
    }

    //현택이 코드
    //     public void ChangeAnimation(PlayerAnimation newAnimation, float normalizedTime = 0)
    // {
    //     animator.Play(newAnimation.ToString(), 0, normalizedTime);
    // }

    // public void ChangeAnimation(string animationName, float normalizedTime = 0)
    // {
    //     animator.Play(animationName, 0, normalizedTime);
    // }
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
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

    // Q 키를 눌렀을 때 TimeStopper 스크립트의 시간 정지 함수 실행
    if (Input.GetKeyDown(KeyCode.Q))
    {
        TimeStopper timeStopper = GetComponent<TimeStopper>();
        if (timeStopper != null)
        {
            timeStopper.StartCoroutine(timeStopper.StopTime());
        }
    }

        // 타임존에 들어가 있으면 애니메이션 속도 감속 및 가속
        if (isInTimeZone)
        {
            animator.speed = animationSpeed;
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

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            // 추가적인 액션이 필요하면 여기에 작성
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

                // y 속도가 특정 기준 이하일 경우(빠르게 낙하한 경우) 플레이어가 피해를 입고 스폰 위치로 이동
                if (yVelocity <= fallDeathVelocity) // 예: fallDeathVelocity = -25
                {
                    // 높은 곳에서 떨어졌을 때만 피해 발생
                    RespawnAtLastSpawnPoint();
                    return; // 리스폰 후 아래 코드를 실행하지 않도록 반환
                }
            }
            isGround = true;
            yVelocity = 0; // 땅에 닿으면 yVelocity를 0으로 초기화
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

        //애니메이션 속도 조절절
        animationSpeed *= speedMultiplier;
        animator.speed = animationSpeed;
        //현택이 코드
        //inTimeZoneSpeed *= speedMultiplier;
        //animator.speed = inTimeZoneSpeed;
    }

    // 플레이어 중력 적용
    public void ApplyGravity()
    {
        if (!isGround)
        {
            yVelocity -= gravity * gravity * Time.deltaTime * speedMultiplier;
            //yVelocity = Mathf.Max(yVelocity, yVelocityMin); // yVelocity의 최소값 적용
            //yVelocity = yVelocity;
            
            if (isInTimeZone)
            {
                yVelocity = Mathf.Max(yVelocity, yVelocityMin);
            }
        }
        Vector3 position = transform.position;

        // yVelocity * Time.deltaTime * speedMultiplier 
        // yVelocity * Time.deltaTime                   
        position.y += yVelocity * Time.deltaTime * speedMultiplier;
        transform.position = position;
    }

    public void RespawnAtLastSpawnPoint()
    {
    
        Transform spawnPoint = PlayerRespawnManager.Instance.GetCurrentSpawnPoint();
        transform.position = spawnPoint.position;
        yVelocity = 0;
        ChangeState(PlayerState.Idle);
    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 스폰 포인트에 도달하면 마지막 스폰 포인트 업데이트
        if (collision.CompareTag("SpawnPoint"))
        {
            // for (int i = 0; i < spawnPoints.Length; i++)
            // {
            //     if (collision.transform == spawnPoints[i])
            //     {
            //         lastSpawnPointIndex = i;
            //         break;
            //     }
            // }
            PlayerRespawnManager.Instance.UpdateSpawnPointIndex(1); // 스폰 포인트 인덱스 1씩 증가

        }
        // SlowTimeZone에 들어갈 때 yVelocity의 최솟값 조정
        else if (collision.CompareTag("SlowTimeZone"))
        {
            yVelocityMin = yVelocityMinSlowTimeZone;
            isInTimeZone = true;
        }
        // FastTimeZone에 들어갈 때 yVelocity의 최솟값 조정
        else if (collision.CompareTag("FastTimeZone"))
        {
            yVelocityMin = yVelocityMinFastTimeZone;
            isInTimeZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 타임존에서 나갈 때 yVelocity의 최솟값을 기본값으로 설정
        if (collision.CompareTag("SlowTimeZone") || collision.CompareTag("FastTimeZone"))
        {
            //yVelocityMin = yVelocityMinDefault;
            isInTimeZone = false;
        }
    }

    //현택이 코드
    //     public void OnDie()
    // {
        
    // }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("BossAttackObjects"))
    //     {
    //         ChangeState(PlayerState.Hit);
    //     }
    // }
}
