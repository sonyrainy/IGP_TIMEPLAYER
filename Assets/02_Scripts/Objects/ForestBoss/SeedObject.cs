using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedObject : TimeZoneObject
{
    [SerializeField] float speed = 0;
    Rigidbody2D rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        float seedVelocityX = Random.Range(1, 2);
        float seedVelocityY = Random.Range(1, 2);
        rigidbody2D.velocity = new Vector2(seedVelocityX, seedVelocityY) * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void AdjustObjectSpeed(float speedMultiplier)
    {
        base.AdjustObjectSpeed(speedMultiplier);

        rigidbody2D.velocity *= speedMultiplier;
    }

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
