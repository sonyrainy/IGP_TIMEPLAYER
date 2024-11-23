using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogObject : MonoBehaviour
{
    [SerializeField] float yVelocity = 0;
    [SerializeField] float gravity = 9.8f;
    [SerializeField] public float speedMultiplier = 1.5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        Vector3 position = transform.position;
        position.y -= yVelocity * Time.deltaTime * speedMultiplier;
        transform.position = position;
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
