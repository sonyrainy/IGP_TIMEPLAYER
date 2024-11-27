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
    [SerializeField] public float moveSpeed = 0; // ?΄λ ?λ
    [SerializeField] public float dashFloat = 0; // ????λ
    [SerializeField] public float jumpForce = 1f; // ?ν ?λ

    [SerializeField] float castSize;
    [SerializeField] float gravity = 9.8f;
    [SerializeField] public float yVelocity = 0; // μ€λ ₯??κ³μ°?κΈ° ?ν yμΆ?λ°©ν₯ ?λ

    [SerializeField] public bool isGround = false; // ?μ λΆμ΄ ?λμ§ ?μΈ
    public bool isDash = false; // ???μ€μΈμ§ ?μΈ

    public State<Player>[] states;

    public float speedMultiplier = 1f; // ???μ‘?μ§μ λ°??μΆ ??κ°μ/κ°???¨κ³Ό λΆ?¬λ? ?ν Float κ°?
    public bool isInTimeZone = false; // ???μ‘΄μ μ§μ?μ??μ? ?μΈ
    public float animationSpeed = 1; // ??μ‘΄ μ§μ λ°??μΆ ??? λλ©μ΄?μ κ°μ/κ°???¨κ³Ό λΆ?¬λ? ?ν Float κ°?
    
    //?ν??μ½λ
    //public float inTimeZoneSpeed = 1;
// μΆκ???λ³?λ€
    public Transform[] spawnPoints; // ?€ν° ?¬μΈ?Έλ€
    private int lastSpawnPointIndex = 0; // λ§μ?λ§μΌλ‘??λ¬???€ν° ?¬μΈ?Έμ ?Έλ±??
    public float fallDeathVelocity = -36.0f; // ???λ ?΄μ?Όλ‘ ?¨μ΄μ§?κ²½μ° ?λ ?΄μ΄κ° μ£½μ


    // yVelocity??μ΅μκ°??€μ 
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

        // ?ν ?νλ‘?λ³κ²½λ  ????λ²λ§ ?ν ?λ¦¬ ?¬μ
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

        // Q ?€λ? ?λ?????TimeStopper ?€ν¬λ¦½νΈ???κ° ?μ? ?¨μ ?€ν
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TimeStopper timeStopper = GetComponent<TimeStopper>();
            if (timeStopper != null)
            {
                timeStopper.StartCoroutine(timeStopper.StopTime());
            }
            Debug.Log("???");
        }

        // ??μ‘΄???€μ΄κ° ?μΌλ©?? λλ©μ΄???λ κ°μ λ°?κ°??
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
            // ????νλ₯??μΈ??λͺ¨λ  ?ν??μ€λ ₯???μ©
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
            // μΆκ??μΈ ?‘μ???μ?λ©΄ ?¬κΈ°???μ±
            // => LeftControl ? ????€μΈ???¬κΈ°???°λΌκ³?
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

    // ?λ ?΄μ΄κ° λ°λ₯??λΆμ΄ ?λμ§ ?μΈ
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

                // y ?λκ° ?Ήμ  κΈ°μ? ?΄ν??κ²½μ°(λΉ λ₯΄κ²??ν??κ²½μ°) ?λ ?΄μ΄κ° ?Όν΄λ₯??κ³  ?€ν° ?μΉλ‘??΄λ
                if (yVelocity <= fallDeathVelocity) // ?? fallDeathVelocity = -25
                {
                    // ?μ? κ³³μ???¨μ΄μ‘μ ?λ§ ?Όν΄ λ°μ
                    RespawnAtLastSpawnPoint();
                    return; // λ¦¬μ€?????λ μ½λλ₯??€ν?μ? ?λλ‘?λ°ν
                }
            }
            isGround = true;
            yVelocity = 0; // ?μ ?ΏμΌλ©?yVelocityλ₯?0?Όλ‘ μ΄κΈ°??
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

        // ?΄λ ?λ μ‘°μ 
        moveSpeed *= speedMultiplier;

        //? λλ©μ΄???λ μ‘°μ 
        animationSpeed *= speedMultiplier;
        animator.speed = animationSpeed;
        //?ν??μ½λ
        //inTimeZoneSpeed *= speedMultiplier;
        //animator.speed = inTimeZoneSpeed;
    }

    // ?λ ?΄μ΄ μ€λ ₯ ?μ©
    public void ApplyGravity()
    {
        if (!isGround)
        {
            yVelocity -= gravity * gravity * Time.deltaTime * speedMultiplier;
            
            //yVelocity = Mathf.Max(yVelocity, yVelocityMin); // yVelocity??μ΅μκ°??μ©
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
        // ?€ν° ?¬μΈ?Έμ ?λ¬?λ©΄ λ§μ?λ§??€ν° ?¬μΈ???λ°?΄νΈ
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
            PlayerRespawnManager.Instance.UpdateSpawnPointIndex(1); // ?€ν° ?¬μΈ???Έλ±??1??μ¦κ?

        }
        // SlowTimeZone???€μ΄κ°???yVelocity??μ΅μκ°?μ‘°μ 
        else if (collision.CompareTag("SlowTimeZone"))
        {
            yVelocityMin = yVelocityMinSlowTimeZone;
            isInTimeZone = true;
        }
        // FastTimeZone???€μ΄κ°???yVelocity??μ΅μκ°?μ‘°μ 
        else if (collision.CompareTag("FastTimeZone"))
        {
            yVelocityMin = yVelocityMinFastTimeZone;
            isInTimeZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // ??μ‘΄?μ ?κ° ??yVelocity??μ΅μκ°μ κΈ°λ³Έκ°μΌλ‘??€μ 
        if (collision.CompareTag("SlowTimeZone") || collision.CompareTag("FastTimeZone"))
        {
            //yVelocityMin = yVelocityMinDefault;
            isInTimeZone = false;
        }
    }

    //?ν??μ½λ
    //     public void OnDie()
    // {
        
    // }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BossAttackObjects"))
        {
            ChangeState(PlayerState.Hit);
        }
    }
}
