using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedObject : TimeZoneObject
{
    [SerializeField] float speed = 0;
    Rigidbody2D rigidbody2D;

    //temp shj
    private Vector2 storedVelocity;
    private float storedVX = 0;
        private float storedVY = 0;

    //~temp shj
    private bool isStopped = false;

//~ temp shj

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        float seedVelocityX = Random.Range(1, 2);
        float seedVelocityY = Random.Range(1, 2);
        storedVX = seedVelocityX;
        storedVY = seedVelocityY;
        rigidbody2D.velocity = new Vector2(seedVelocityX, seedVelocityY) * speed;
    }



    public override void AdjustObjectSpeed(float speedMultiplier)
    {
        base.AdjustObjectSpeed(speedMultiplier);

//            rigidbody2D.velocity *= speedMultiplier;

        //temp shj
      if (!isStopped)
        {
            rigidbody2D.velocity *= speedMultiplier;
        }   
        
        //~temp shj
        
     }

//temp shj~
    public void StopMovement(float duration)
    {
            Debug.Log("StopMovement called on SeedObject.");

        if (!isStopped)
        {
            StartCoroutine(StopMovementCoroutine(duration));
        }
    }
    //~temp shj

//temp shj~
   private IEnumerator StopMovementCoroutine(float duration)
    {
        isStopped = true;
        //storedVelocity = rigidbody2D.velocity; // 2D 벡터로 전체 속도 저장
        rigidbody2D.velocity = Vector2.zero; // 속도 초기화

        
        yield return new WaitForSeconds(duration);
        
      // 저장된 속도로 복원
        //rigidbody2D.velocity = storedVelocity;
        float seedVelocityX = storedVX;
        float seedVelocityY = storedVY;

        rigidbody2D.velocity = new Vector2(-seedVelocityX, -seedVelocityY) * speed;
        Debug.Log($"Restored velocity: {storedVelocity}");

        isStopped = false;
    }
// ~temp shj
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            BossSceneManager.Instance.isSeedSpawned = false;
            Destroy(gameObject);
            BossSceneManager.Instance.SetActiveTrueTreeAttackObject();
        }
    }
}
