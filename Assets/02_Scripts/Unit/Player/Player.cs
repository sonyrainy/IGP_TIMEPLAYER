using Player_States;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : TimeZoneObject
{
    public PlayerState prevPlayerState = PlayerState.Idle;
    public PlayerState playerState = PlayerState.Idle;

    public Animator animator;

    [SerializeField] LayerMask floorLayer;
    [SerializeField] public float moveSpeed = 0; // 이동 속도
    [SerializeField] public float dashFloat = 0; // 대쉬 속도
    [SerializeField] public float jumpForce = 1f; // 점프 속도
    [SerializeField] float castSize;
    [SerializeField] float gravity = 9.8f;
    [SerializeField] public float yVelocity = 0; // 중력을 계산하기 위한 y축 방향 속도
    [SerializeField] public bool isGround = false; // 땅에 붙어 있는지 확인

    public bool isDash = false; // 대쉬중인지 확인
    public State<Player>[] states;
    public float animationSpeed = 1; // ?�?�존 진입 �??�출 ???�니메이?�의 감속/가???�과 부?��? ?�한 Float �?
// 추�???변?�들
    public Transform[] spawnPoints; // ?�폰 ?�인?�들
    private int lastSpawnPointIndex = 0; // 마�?막으�??�달???�폰 ?�인?�의 ?�덱??
    public float fallDeathVelocity = -36.0f; // ???�도 ?�상?�로 ?�어�?경우 ?�레?�어가 죽음
    private GameObject timeStopEffect;


    // yVelocity??최소�??�정
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

        // ?�프 ?�태�?변경될 ????번만 ?�프 ?�리 ?�생
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

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        // TimeStopEffect 자식 오브젝트 찾기
        timeStopEffect = transform.Find("TimeStopEffect").gameObject;
        if (timeStopEffect != null)
        {
            timeStopEffect.SetActive(false);
        }
    }

    private void OnEnable()
    {
        InitFSM();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TimeStopper timeStopper = GetComponent<TimeStopper>();
            if (timeStopper != null)
            {
                timeStopper.StartCoroutine(timeStopper.StopTime());
                timeStopper.StartCoroutine(timeStopper.ActivateTimeStopEffect(timeStopEffect));
            }
            Debug.Log("?�??");
        }



        // 타임 존에 들어가 있으면 애니메이션 속도 감속 및 가속
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
        states[(int)PlayerState.Fall] = new Fall(this);

        states[(int)playerState].Enter();
    }

    // 플레이어가 바닥에 붙어 있는지 확인
    void GroundCheck()
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

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

                // y ?�도가 ?�정 기�? ?�하??경우(빠르�??�하??경우) ?�레?�어가 ?�해�??�고 ?�폰 ?�치�??�동
                if (yVelocity <= fallDeathVelocity) // ?? fallDeathVelocity = -25
                {
                    // ?��? 곳에???�어졌을 ?�만 ?�해 발생
                    RespawnAtLastSpawnPoint();
                    return; // 리스?????�래 코드�??�행?��? ?�도�?반환
                }
            }
            isGround = true;
            yVelocity = 0; // ?�에 ?�으�?yVelocity�?0?�로 초기??
        }
        else
        {
            isGround = false;
        }
    }
}
    public override void AdjustObjectSpeed(float speedMultiplier)
    {
        this.speedMultiplier *= speedMultiplier;

        moveSpeed *= speedMultiplier;

        animationSpeed *= speedMultiplier;
        animator.speed = animationSpeed;
    }

    // 플레이어 중력 적용
    public void ApplyGravity()
    {
        if (!isGround)
        {
            yVelocity -= gravity * gravity * Time.deltaTime * speedMultiplier;
            
            //yVelocity = Mathf.Max(yVelocity, yVelocityMin); // yVelocity??최소�??�용
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
        // ?�폰 ?�인?�에 ?�달?�면 마�?�??�폰 ?�인???�데?�트
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
            PlayerRespawnManager.Instance.UpdateSpawnPointIndex(1); // ?�폰 ?�인???�덱??1??증�?

        }
        // SlowTimeZone???�어�???yVelocity??최솟�?조정
        else if (collision.CompareTag("SlowTimeZone"))
        {
            yVelocityMin = yVelocityMinSlowTimeZone;
            isInTimeZone = true;
        }
        // FastTimeZone???�어�???yVelocity??최솟�?조정
        else if (collision.CompareTag("FastTimeZone"))
        {
            yVelocityMin = yVelocityMinFastTimeZone;
            isInTimeZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // ?�?�존?�서 ?�갈 ??yVelocity??최솟값을 기본값으�??�정
        if (collision.CompareTag("SlowTimeZone") || collision.CompareTag("FastTimeZone"))
        {
            //yVelocityMin = yVelocityMinDefault;
            isInTimeZone = false;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BossAttackObjects"))
        {
            ChangeState(PlayerState.Hit);
            
            PlayerHealth playerHealth = GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1); // 1만큼의 피해
            }
        }
    }
}
