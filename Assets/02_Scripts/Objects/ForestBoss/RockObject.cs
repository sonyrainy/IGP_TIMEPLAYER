using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RockObject : TimeZoneObject
{
    Transform transform;
    public float xVelocity = 0;
    public float acceleration = 0.5f; // 가속도
    public float destroyXPosition = -10f; // 파괴될 X축 위치
    public bool isInTimeZone = false;

    void Start()
    {
        transform = GetComponent<Transform>();
    }

    void Update()
    {
        HorizontalMove();
    }

    private void HorizontalMove()
    {
        xVelocity -= Time.deltaTime * speedMultiplier;
        xVelocity = Mathf.Max(xVelocity, -10);

        Vector3 position = transform.position;

        position.x += xVelocity * Time.deltaTime * speedMultiplier;
        transform.position = position;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BossAttackObjects"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
