using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeZoneEffect : MonoBehaviour
{
    //Timezone 들어갔을 때 속도 얼만큼 변하게 되도록 할지 배율
    public float speedMultiplier = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            Debug.Log("Player TimeZone 입장");

            if (player != null)
            {
                player.moveSpeed *= speedMultiplier;
                
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            Debug.Log("Player TimeZone 퇴장");

            if (player != null)
            {
            player.moveSpeed /= speedMultiplier;
            }
        }
    }
    
}
