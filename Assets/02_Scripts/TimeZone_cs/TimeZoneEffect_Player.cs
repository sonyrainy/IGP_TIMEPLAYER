using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeZoneEffect_Player : MonoBehaviour
{
    private float speedMultiplier;
    
    //굳이 안쓰고 위에거 public해도 되긴 함.
    public float SpeedMultiplier
    {
        get { return speedMultiplier; }
        set { speedMultiplier = value; }
    }
    
    private float linearDragValue;
    
    public float LinearDragValue
    {
        get { return linearDragValue; }
        set { linearDragValue = value; }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            Debug.Log("Player TimeZone 입장");

            if (player != null)
            {
                player.moveSpeed *= speedMultiplier;
                player.GetComponent<Rigidbody2D>().drag = linearDragValue;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            Debug.Log("Player TimeZone 퇴장");

            //일반 값으로 복귀
            if (player != null)
            {
                player.moveSpeed /= speedMultiplier;
                player.GetComponent<Rigidbody2D>().drag = 3f;
            }
        }
    }
}
