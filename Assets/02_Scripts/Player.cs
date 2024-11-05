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

    [SerializeField] public float jumpForce = 10;

    [SerializeField] float castSize;

    bool isGround = true;

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
        GroundCheck();

        float i = Input.GetAxisRaw("Horizontal");

        if(!isGround)
            ani.SetFloat("YSpeed", rigidbody.velocity.y);

        if(i != 0)
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
        vel.y = jumpForce;
        rigidbody.velocity = vel;
        isGround = false;
        ani.SetTrigger("Jump");
    }

    void Dash()
    {

    }
}
