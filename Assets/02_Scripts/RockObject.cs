using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockObject : MonoBehaviour
{
    public float initialSpeed = 5f; // 초기 이동 속도
    public float acceleration = 0.5f; // 가속도 (초당 속도 증가량)
    public float destroyXPosition = -10f; // 파괴될 X축 위치

    private float currentSpeed; // 현재 속도

    void Start()
    {
        // 초기 속도를 설정
        currentSpeed = initialSpeed;
    }

    void Update()
    {
        HorizontalMove();
    }

    private void HorizontalMove()
    {
        // 속도를 점점 증가시킴
        currentSpeed += acceleration * Time.deltaTime;

        // 오른쪽에서 왼쪽으로 이동
        transform.Translate(Vector3.left * currentSpeed * Time.deltaTime);

        // 지정된 X축 위치에 도달하면 오브젝트를 파괴
        if (transform.position.x <= destroyXPosition)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 오브젝트의 태그 확인
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
