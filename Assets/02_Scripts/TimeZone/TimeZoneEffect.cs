using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeZoneEffect : MonoBehaviour
{
    // Timezone 들어갔을 때 속도 얼만큼 변하게 되도록 할지 배율
    // 타임존마다 prefab에서 다르게 설정되어 있음.
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
        else if (other.CompareTag("F_Tree")) // FallingTree의 경우
        {
            FallingTree fallingTree = other.GetComponent<FallingTree>();

            Debug.Log("FallingTree TimeZone 입장");

            if (fallingTree != null)
            {
                fallingTree.AdjustFallSpeed(speedMultiplier);
                fallingTree.EnterTimeZone(tag); // 현재 타임존의 태그를 전달하여 적절한 행동 실행
            }
        }
        // GrowingTree의 경우
        else if (other.CompareTag("GrowingTree")&& CompareTag("FastTimeZone"))
        {
            GrowingTree growingTree = other.GetComponent<GrowingTree>();

            Debug.Log("GrowingTree TimeZone 입장");

            if (growingTree != null)
            {
                growingTree.EnterFastTimeZone();
            }
        } 
        else if (other.CompareTag("MovingPlatform"))
        {
            MovingTilemap movingPlatform = other.GetComponent<MovingTilemap>();

            Debug.Log("MovingPlatform TimeZone 입장");

            if (movingPlatform != null)
            {
                movingPlatform.AdjustSpeed(speedMultiplier);
                movingPlatform.EnterTimeZone(tag);
            }
        }
        
        if (other.CompareTag("BossAttackObjects"))
        {           
            RockObject rock = other.GetComponent<RockObject>();
            LogObject log = other.GetComponent<LogObject>();
            
            if (rock != null)
            {
                rock.AdjustObjectSpeed(speedMultiplier);
                rock.isInTimeZone = true;
            }

            if (log != null)
            {
                log.AdjustObjectSpeed(speedMultiplier);
                log.isInTimeZone = true;
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
        else if (other.CompareTag("F_Tree")) // FallingTree의 경우
        {
            FallingTree fallingTree = other.GetComponent<FallingTree>();

            Debug.Log("FallingTree TimeZone 퇴장");

            if (fallingTree != null)
            {
                fallingTree.AdjustFallSpeed(1f / speedMultiplier);
                fallingTree.ExitTimeZone();
            }
        }
        // GrowingTree의 경우
        else if (other.CompareTag("GrowingTree")&& CompareTag("FastTimeZone"))
        {
            GrowingTree growingTree = other.GetComponent<GrowingTree>();

            Debug.Log("GrowingTree TimeZone 퇴장");

            if (growingTree != null)
            {
                growingTree.ExitFastTimeZone();
            }
        }
        else if (other.CompareTag("MovingPlatform"))
        {
            MovingTilemap movingPlatform = other.GetComponent<MovingTilemap>();

            Debug.Log("MovingPlatform TimeZone 퇴장");

            if (movingPlatform != null)
            {
                movingPlatform.AdjustSpeed(1/speedMultiplier);
                movingPlatform.ExitTimeZone();
            }
        }



        if (other.CompareTag("BossAttackObjects"))
        {           
            RockObject rock = other.GetComponent<RockObject>();
            LogObject log = other.GetComponent<LogObject>();
            
            if (rock != null)
            {
                rock.AdjustObjectSpeed(1f / speedMultiplier);
                rock.isInTimeZone = false;
            }

            if (log != null)
            {
                log.AdjustObjectSpeed(1f / speedMultiplier);
                log.isInTimeZone = false;
            }
        }
    }
    
}
