using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTree : MonoBehaviour
{
    private Rigidbody2D rb;
    private float fallSpeed;
    private float originalSpeed;
    public float originalLinearDrag { get; private set; }
    private bool isStopped = false;
    private bool isInTimeZone = false; // TimeZone에 있는지 여부

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        rb.bodyType = RigidbodyType2D.Dynamic; // Rigidbody 타입을 Dynamic으로 설정
        rb.gravityScale = 0; // 중력은 코드로 처리하므로 0으로 설정
        originalLinearDrag = rb.drag;
    }

    public void SetFallSpeed(float speed)
    {
        fallSpeed = speed;
        originalSpeed = speed; // 원래 낙하 속도를 저장
    }

    public void AdjustFallSpeed(float speedMultiplier)
    {
        fallSpeed = originalSpeed * speedMultiplier;
    }

    public void AdjustLinearDrag(float dragValue)
    {
        if (rb != null)
        {
            rb.drag = dragValue;
        }
    }

    public void StopPlatformForDuration(float duration)
    {
        if (!isStopped)
        {
            StartCoroutine(StopMovement(duration));
        }
    }

    private IEnumerator StopMovement(float duration)
    {
        isStopped = true;

        // 나무의 속도와 중력을 멈춤
        Vector2 originalVelocity = rb.velocity;
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForSeconds(duration);

        // 정지 해제
        rb.constraints = RigidbodyConstraints2D.None;
        rb.velocity = originalVelocity;
        isStopped = false;
    }

    void Update()
    {
        // 천천히 떨어지기 위해 SlowTimeZone에 있을 때 낙하 속도를 조정
        if (!isStopped)
        {
            rb.velocity = new Vector2(rb.velocity.x, -fallSpeed); // Rigidbody를 통해 떨어지도록 설정
        }
    }

    public void EnterTimeZone(string zoneType)
    {
        isInTimeZone = true;
        if (zoneType == "SlowTimeZone")
        {
            AdjustLinearDrag(originalLinearDrag * 80.0f);
        }
        else if (zoneType == "FastTimeZone")
        {
            AdjustLinearDrag(originalLinearDrag * 0.2f);
        }
    }

    public void ExitTimeZone()
    {
        isInTimeZone = false;
        AdjustLinearDrag(originalLinearDrag);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // 플레이어와 충돌 시 현재 리스폰 포인트로 이동
            Player player = collision.collider.GetComponent<Player>();
            if (player != null)
            {
                player.RespawnAtLastSpawnPoint();
            }
        }
        else if (collision.collider.CompareTag("Ground"))
        {
            // 땅과 충돌 시 나무 삭제
            Destroy(gameObject); // 나무 오브젝트를 삭제
        }
    }
}
