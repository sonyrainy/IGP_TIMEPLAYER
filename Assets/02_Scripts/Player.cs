using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator ani;
    private Transform transform;
    private Rigidbody2D rigidbody;

    [SerializeField] LayerMask floorLayer;
    [SerializeField] public float moveSpeed = 0;
    [SerializeField] public float dashSpeed = 0;
    [SerializeField] public float jumpForce = 1f;

    [SerializeField] float castSize;
    [SerializeField] float gravity = 9.8f;
    [SerializeField] float yVelocity = 0;

    [SerializeField] bool isGround = false;
    [SerializeField] bool isDash = false;

    public float speedMultiplier = 1f;
    public bool isInTimeZone = false; 
    public float animationSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        ani =  GetComponentInChildren<Animator>();
        transform = GetComponentInChildren<Transform>();
        rigidbody = GetComponentInChildren<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // 타임존에 있는 경우 애니메이터 속도 조절
        if (isInTimeZone)
        {
            ani.speed = animationSpeed;
        }
        else
        {
            ani.speed = 1.0f;
        }

        GroundCheck();

        ApplyGravity();

        float playerDirection = Input.GetAxisRaw("Horizontal");

        if (!isGround)
        {
            ani.SetFloat("YSpeed", yVelocity);
        }

        if (playerDirection != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = playerDirection;
            transform.localScale = scale;
        }

        Vector3 vel = rigidbody.velocity;
        vel.x = moveSpeed * playerDirection;
        rigidbody.velocity = vel;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            ani.SetTrigger("Dash");
            Dash(playerDirection);
        }

        ani.SetBool("Run", playerDirection != 0);
    }

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
                    ani.SetTrigger("OnGround");
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

    void Jump()
    {
        if (!isGround) return;

        yVelocity = jumpForce;
        isGround = false;
        ani.SetTrigger("Jump");
    }

    public void AdjustObjectSpeed(float speedMultiplier)
    {
        this.speedMultiplier *= speedMultiplier;

        // 이동 속도 조정
        moveSpeed *= speedMultiplier;

        // 애니메이터 속도 조정
        animationSpeed *= speedMultiplier;
        ani.speed = animationSpeed;
    }

    private void ApplyGravity()
    {
        if (!isGround)
        {
            yVelocity -= gravity * gravity * Time.deltaTime * speedMultiplier;
            yVelocity = Mathf.Max(yVelocity, -20);
        }
        Vector3 position = transform.position;

        // yVelocity * Time.deltaTime * speedMultiplier => 높이 고정
        // yVelocity * Time.deltaTime => 시간 느릴 땐 낮게, 시간 빠를 땐 높게
        position.y += yVelocity * Time.deltaTime * speedMultiplier;
        transform.position = position;
    }

    void Dash(float direction)
    {
        if (!isGround)
        {
            yVelocity = 0;
        }

        Vector3 vel = rigidbody.velocity;
        vel.x = moveSpeed * direction * dashSpeed;
        rigidbody.velocity = vel;
    }
}
