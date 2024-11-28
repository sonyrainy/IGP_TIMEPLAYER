using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeZoneEffect : MonoBehaviour
{
    // Timezone ?�어갔을 ???�도 ?�만??변?�게 ?�도�??��? 배율
    // ?�?�존마다 prefab?�서 ?�르�??�정?�어 ?�음.
    public float speedMultiplier = 1f;

    public void setSpeedMultiplier (float speedMultiplier)
    {
        this.speedMultiplier = speedMultiplier;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();

            Debug.Log("Player TimeZone ?�장");

            if (player != null)
            {
                player.AdjustObjectSpeed(speedMultiplier);
                player.isInTimeZone = true;
            }
        }
        else if (other.CompareTag("F_Tree")) // FallingTree??경우
        {
            FallingTree fallingTree = other.GetComponent<FallingTree>();

            Debug.Log("FallingTree TimeZone ?�장");

            if (fallingTree != null)
            {
                fallingTree.AdjustFallSpeed(speedMultiplier);
                fallingTree.EnterTimeZone(tag); // ?�재 ?�?�존???�그�??�달?�여 ?�절???�동 ?�행
            }
        }
        // GrowingTree??경우
        else if (other.CompareTag("GrowingTree")&& CompareTag("FastTimeZone"))
        {
            GrowingTree growingTree = other.GetComponent<GrowingTree>();

            Debug.Log("GrowingTree TimeZone ?�장");

            if (growingTree != null)
            {
                growingTree.EnterFastTimeZone();
            }
        } 
        else if (other.CompareTag("MovingPlatform"))
        {
            MovingTilemap movingPlatform = other.GetComponent<MovingTilemap>();

            Debug.Log("MovingPlatform TimeZone ?�장");

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

        if (other.attachedRigidbody.CompareTag("PlayerTreeAttackObjects"))
        {
            PlayerTreeAttackObject playerTreeAttackObject = other.attachedRigidbody.GetComponent<PlayerTreeAttackObject>();

            if (playerTreeAttackObject != null)
            {
                playerTreeAttackObject.AdjustObjectSpeed(speedMultiplier);
                playerTreeAttackObject.isInTimeZone = true;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();

            Debug.Log("Player TimeZone ?�장");

            if (player != null)
            {
                player.AdjustObjectSpeed(1f / speedMultiplier);
                player.isInTimeZone = false;
            }
        }
        else if (other.CompareTag("F_Tree")) // FallingTree??경우
        {
            FallingTree fallingTree = other.GetComponent<FallingTree>();

            Debug.Log("FallingTree TimeZone ?�장");

            if (fallingTree != null)
            {
                fallingTree.AdjustFallSpeed(1f / speedMultiplier);
                fallingTree.ExitTimeZone();
            }
        }
        // GrowingTree??경우
        else if (other.CompareTag("GrowingTree")&& CompareTag("FastTimeZone"))
        {
            GrowingTree growingTree = other.GetComponent<GrowingTree>();

            Debug.Log("GrowingTree TimeZone ?�장");

            if (growingTree != null)
            {
                growingTree.ExitFastTimeZone();
            }
        }
        else if (other.CompareTag("MovingPlatform"))
        {
            MovingTilemap movingPlatform = other.GetComponent<MovingTilemap>();

            Debug.Log("MovingPlatform TimeZone ?�장");

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

        if (other.attachedRigidbody.CompareTag("PlayerTreeAttackObjects"))
        {
            PlayerTreeAttackObject playerTreeAttackObject = other.attachedRigidbody.GetComponent<PlayerTreeAttackObject>();

            if (playerTreeAttackObject != null)
            {
                playerTreeAttackObject.AdjustObjectSpeed(1f / speedMultiplier);
                playerTreeAttackObject.isInTimeZone = false;
            }
        }
    }
    
}
