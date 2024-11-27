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
    [SerializeField] public float moveSpeed = 0; // ?´ë™ ?ë„
    [SerializeField] public float dashFloat = 0; // ?€???ë„
    [SerializeField] public float jumpForce = 1f; // ?í”„ ?ë„

    [SerializeField] float castSize;
    [SerializeField] float gravity = 9.8f;
    [SerializeField] public float yVelocity = 0; // ì¤‘ë ¥??ê³„ì‚°?˜ê¸° ?„í•œ yì¶?ë°©í–¥ ?ë„

    [SerializeField] public bool isGround = false; // ?…ì— ë¶™ì–´ ?ˆëŠ”ì§€ ?•ì¸
    public bool isDash = false; // ?€??ì¤‘ì¸ì§€ ?•ì¸

    public State<Player>[] states;

    public float speedMultiplier = 1f; // ?€??ì¡?ì§„ì… ë°??ˆì¶œ ??ê°ì†/ê°€???¨ê³¼ ë¶€?¬ë? ?„í•œ Float ê°?
    public bool isInTimeZone = false; // ?€??ì¡´ì— ì§„ì…?˜ì??”ì? ?•ì¸
    public float animationSpeed = 1; // ?€?„ì¡´ ì§„ì… ë°??ˆì¶œ ??? ë‹ˆë©”ì´?˜ì˜ ê°ì†/ê°€???¨ê³¼ ë¶€?¬ë? ?„í•œ Float ê°?
    
    //?„íƒ??ì½”ë“œ
    //public float inTimeZoneSpeed = 1;
// ì¶”ê???ë³€?˜ë“¤
    public Transform[] spawnPoints; // ?¤í° ?¬ì¸?¸ë“¤
    private int lastSpawnPointIndex = 0; // ë§ˆì?ë§‰ìœ¼ë¡??„ë‹¬???¤í° ?¬ì¸?¸ì˜ ?¸ë±??
    public float fallDeathVelocity = -36.0f; // ???ë„ ?´ìƒ?¼ë¡œ ?¨ì–´ì§?ê²½ìš° ?Œë ˆ?´ì–´ê°€ ì£½ìŒ


    // yVelocity??ìµœì†Œê°??¤ì •
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

        // ?í”„ ?íƒœë¡?ë³€ê²½ë  ????ë²ˆë§Œ ?í”„ ?Œë¦¬ ?¬ìƒ
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

        // Q ?¤ë? ?Œë?????TimeStopper ?¤í¬ë¦½íŠ¸???œê°„ ?•ì? ?¨ìˆ˜ ?¤í–‰
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TimeStopper timeStopper = GetComponent<TimeStopper>();
            if (timeStopper != null)
            {
                timeStopper.StartCoroutine(timeStopper.StopTime());
            }
            Debug.Log("?€??");
        }

        // ?€?„ì¡´???¤ì–´ê°€ ?ˆìœ¼ë©?? ë‹ˆë©”ì´???ë„ ê°ì† ë°?ê°€??
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
            // ?€???íƒœë¥??œì™¸??ëª¨ë“  ?íƒœ??ì¤‘ë ¥???ìš©
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
            // ì¶”ê??ì¸ ?¡ì…˜???„ìš”?˜ë©´ ?¬ê¸°???‘ì„±
            // => LeftControl ?€ ?€???¤ì¸???¬ê¸°???°ë¼ê³?
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

    // ?Œë ˆ?´ì–´ê°€ ë°”ë‹¥??ë¶™ì–´ ?ˆëŠ”ì§€ ?•ì¸
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

                // y ?ë„ê°€ ?¹ì • ê¸°ì? ?´í•˜??ê²½ìš°(ë¹ ë¥´ê²??™í•˜??ê²½ìš°) ?Œë ˆ?´ì–´ê°€ ?¼í•´ë¥??…ê³  ?¤í° ?„ì¹˜ë¡??´ë™
                if (yVelocity <= fallDeathVelocity) // ?? fallDeathVelocity = -25
                {
                    // ?’ì? ê³³ì—???¨ì–´ì¡Œì„ ?Œë§Œ ?¼í•´ ë°œìƒ
                    RespawnAtLastSpawnPoint();
                    return; // ë¦¬ìŠ¤?????„ë˜ ì½”ë“œë¥??¤í–‰?˜ì? ?Šë„ë¡?ë°˜í™˜
                }
            }
            isGround = true;
            yVelocity = 0; // ?…ì— ?¿ìœ¼ë©?yVelocityë¥?0?¼ë¡œ ì´ˆê¸°??
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

        // ?´ë™ ?ë„ ì¡°ì •
        moveSpeed *= speedMultiplier;

        //? ë‹ˆë©”ì´???ë„ ì¡°ì ˆ
        animationSpeed *= speedMultiplier;
        animator.speed = animationSpeed;
        //?„íƒ??ì½”ë“œ
        //inTimeZoneSpeed *= speedMultiplier;
        //animator.speed = inTimeZoneSpeed;
    }

    // ?Œë ˆ?´ì–´ ì¤‘ë ¥ ?ìš©
    public void ApplyGravity()
    {
        if (!isGround)
        {
            yVelocity -= gravity * gravity * Time.deltaTime * speedMultiplier;
            
            //yVelocity = Mathf.Max(yVelocity, yVelocityMin); // yVelocity??ìµœì†Œê°??ìš©
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
        // ?¤í° ?¬ì¸?¸ì— ?„ë‹¬?˜ë©´ ë§ˆì?ë§??¤í° ?¬ì¸???…ë°?´íŠ¸
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
            PlayerRespawnManager.Instance.UpdateSpawnPointIndex(1); // ?¤í° ?¬ì¸???¸ë±??1??ì¦ê?

        }
        // SlowTimeZone???¤ì–´ê°???yVelocity??ìµœì†Ÿê°?ì¡°ì •
        else if (collision.CompareTag("SlowTimeZone"))
        {
            yVelocityMin = yVelocityMinSlowTimeZone;
            isInTimeZone = true;
        }
        // FastTimeZone???¤ì–´ê°???yVelocity??ìµœì†Ÿê°?ì¡°ì •
        else if (collision.CompareTag("FastTimeZone"))
        {
            yVelocityMin = yVelocityMinFastTimeZone;
            isInTimeZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // ?€?„ì¡´?ì„œ ?˜ê°ˆ ??yVelocity??ìµœì†Ÿê°’ì„ ê¸°ë³¸ê°’ìœ¼ë¡??¤ì •
        if (collision.CompareTag("SlowTimeZone") || collision.CompareTag("FastTimeZone"))
        {
            //yVelocityMin = yVelocityMinDefault;
            isInTimeZone = false;
        }
    }

    //?„íƒ??ì½”ë“œ
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
