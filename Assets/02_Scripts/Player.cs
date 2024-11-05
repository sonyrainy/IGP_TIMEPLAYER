using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Time
{
    private Animator ani;
    private Transform transform;
    private Rigidbody2D rigidbody;

    [SerializeField] LayerMask floorLayer;
    [SerializeField] public float moveSpeed = 0;
    [SerializeField] public float dashSpeed = 0;
    [SerializeField] public float jumpForce = 1f;
    [SerializeField] private float defaultGravityScale = 1f; // 기본 중력 값
    [SerializeField] private float targetJumpHeight = 5f; // 목표 점프 높이 (일정하게 유지하고자 하는 높이)

    [SerializeField] float castSize;

    bool isGround = true;

    public bool isInTimeZone = false; 
    public float animationSpeed = 1;

    private float originalJumpForce; // 원래 점프 힘 저장

    // Start is called before the first frame update
    void Start()
    {
        ani =  GetComponentInChildren<Animator>();
        transform = GetComponentInChildren<Transform>();
        rigidbody = GetComponentInChildren<Rigidbody2D>();

        // 원래 점프 힘을 저장해둡니다.
        originalJumpForce = jumpForce;
        rigidbody.gravityScale = defaultGravityScale; // 기본 중력 설정
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();

        float i = Input.GetAxisRaw("Horizontal");

        if (!isGround)
        {
            ani.SetFloat("YSpeed", rigidbody.velocity.y);
        }

        if (i != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = i;
            transform.localScale = scale;
        }

        Vector3 vel = rigidbody.velocity;
        vel.x = moveSpeed * i;
        rigidbody.velocity = vel;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            ani.SetTrigger("Dash");
        }

        ani.SetBool("Run", i != 0);

        // 타임존에 있는 경우 애니메이터 속도 조절
        if (isInTimeZone)
        {
            ani.speed = animationSpeed;
        }
        else
        {
            ani.speed = 1.0f;
        }

    }

    void GroundCheck()
    {
        if (isGround) return;
        if (rigidbody.velocity.y < 0)
        {
            Debug.DrawLine(rigidbody.position, rigidbody.position+(Vector2.down* castSize), Color.red);

            RaycastHit2D rayHit = Physics2D.Raycast(rigidbody.position, Vector3.down, castSize, floorLayer);
            if (rayHit.collider != null)
            {
                isGround = true;
                ani.SetTrigger("OnGround");
            }
        }
    }

    void Jump()
    {
        if (!isGround) return;

        Vector3 vel = rigidbody.velocity;
        vel.y = 10 * jumpForce;
        rigidbody.velocity = vel;
        isGround = false;
        ani.SetTrigger("Jump");
    }

    public void AdjustObjectSpeed(float speedMultiplier)
    {
        // 이동 속도 조정
        moveSpeed *= speedMultiplier;

        // 애니메이터 속도 조정
        animationSpeed *= speedMultiplier;
        ani.speed = animationSpeed;

        // 중력 및 점프력 보정
        AdjustGravity(speedMultiplier);
    }

    private void AdjustGravity(float speedMultiplier)
    {
    }

    void Dash()
    {

    }
}
