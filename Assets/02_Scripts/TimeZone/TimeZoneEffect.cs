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
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();

            Debug.Log("Player TimeZone 입장");

            if (player != null)
            {
                player.AdjustObjectSpeed(speedMultiplier);
                player.isInTimeZone = true;

            }
        }
                else if (other.CompareTag("F_Tree")) // 나무도 천천히 떨어지도록 설정
        {
            FallingTree fallingTree = other.GetComponent<FallingTree>();

            Debug.Log("Tree TimeZone 입장");

            if (fallingTree != null)
            {
                fallingTree.AdjustFallSpeed(speedMultiplier);
            }
        }
                // GrowingTree의 경우
        else if (other.CompareTag("GrowingTree"))
        {
            GrowingTree growingTree = other.GetComponent<GrowingTree>();

            Debug.Log("GrowingTree TimeZone 입장");

            if (growingTree != null)
            {
                growingTree.EnterFastTimeZone();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();


            Debug.Log("Player TimeZone 퇴장"); 

            if (player != null)
            {
                player.AdjustObjectSpeed(1f / speedMultiplier);
                player.isInTimeZone = false;


            }
        }
                else if (other.CompareTag("F_Tree")) // 나무가 타임존을 벗어나면 다시 원래 속도로
        {
            FallingTree fallingTree = other.GetComponent<FallingTree>();

            Debug.Log("Tree TimeZone 퇴장");

            if (fallingTree != null)
            {
                fallingTree.AdjustFallSpeed(1f / speedMultiplier);
            }
        }
        // GrowingTree의 경우
        else if (other.CompareTag("GrowingTree"))
        {
            GrowingTree growingTree = other.GetComponent<GrowingTree>();

            Debug.Log("GrowingTree TimeZone 퇴장");

            if (growingTree != null)
            {
                growingTree.ExitFastTimeZone();
            }
        }

    }
    
}
