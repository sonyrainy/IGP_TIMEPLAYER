using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeZoneObject : MonoBehaviour
{
    public float speedMultiplier = 1f;

    public void AdjustObjectSpeed(float speedMultiplier)
    {
        this.speedMultiplier = speedMultiplier;
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
