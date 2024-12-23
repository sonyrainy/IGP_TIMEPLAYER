using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogObject : TimeZoneObject
{
    [SerializeField] float yVelocity = 0;
    [SerializeField] float gravity = 9.8f;

    private float originalGravity;
    private float originalYVelocity;
    private bool isStopped = false;

    // Start is called before the first frame update
    void Start()
    {
        originalGravity = gravity;
        originalYVelocity = yVelocity;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isStopped) 
        {
            ApplyGravity();
        }
    }

    private void ApplyGravity()
    {
        Vector3 position = transform.position;
        if (!isStopped)
        {
            position.y -= yVelocity * gravity * Time.deltaTime * speedMultiplier;
            transform.position = position;
        }
    }

    public void StopMovement(float duration)
    {
        if (!isStopped)
        {
            StartCoroutine(StopMovementCoroutine(duration));
        }
    }

    private IEnumerator StopMovementCoroutine(float duration)
    {
        isStopped = true; 

        // 중력과 yVelocity를 0으로 설정하여 오브젝트가 멈추게 함
        yVelocity = 0;
        gravity = 0;

        yield return new WaitForSeconds(duration); 

        gravity = originalGravity;

        yVelocity = originalYVelocity; 
        isStopped = false; 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BossAttackObjects"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
        else
        {

            isStopped = false;

            Destroy(gameObject);
        }


        //hyuntaek
        //Destroy(gameobject);
    }
}