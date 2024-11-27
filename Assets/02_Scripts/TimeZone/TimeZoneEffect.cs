using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeZoneEffect : MonoBehaviour
{
    // Timezone ?€μ΄κ°μ ???λ ?Όλ§??λ³?κ² ?λλ‘?? μ? λ°°μ¨
    // ??μ‘΄λ§λ€ prefab?μ ?€λ₯΄κ²??€μ ?μ΄ ?μ.
    public float speedMultiplier = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();

            Debug.Log("Player TimeZone ?μ₯");

            if (player != null)
            {
                player.AdjustObjectSpeed(speedMultiplier);
                player.isInTimeZone = true;
            }
        }
        else if (other.CompareTag("F_Tree")) // FallingTree??κ²½μ°
        {
            FallingTree fallingTree = other.GetComponent<FallingTree>();

            Debug.Log("FallingTree TimeZone ?μ₯");

            if (fallingTree != null)
            {
                fallingTree.AdjustFallSpeed(speedMultiplier);
                fallingTree.EnterTimeZone(tag); // ?μ¬ ??μ‘΄???κ·Έλ₯??λ¬?μ¬ ?μ ???λ ?€ν
            }
        }
        // GrowingTree??κ²½μ°
        else if (other.CompareTag("GrowingTree")&& CompareTag("FastTimeZone"))
        {
            GrowingTree growingTree = other.GetComponent<GrowingTree>();

            Debug.Log("GrowingTree TimeZone ?μ₯");

            if (growingTree != null)
            {
                growingTree.EnterFastTimeZone();
            }
        } 
        else if (other.CompareTag("MovingPlatform"))
        {
            MovingTilemap movingPlatform = other.GetComponent<MovingTilemap>();

            Debug.Log("MovingPlatform TimeZone ?μ₯");

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


        Debug.Log("1111");
        Debug.Log(other.gameObject.name);
        if (other.attachedRigidbody.CompareTag("PlayerTreeAttackObjects"))
        {
            Debug.Log("2222");

            PlayerTreeAttackObject playerTreeAttackObject = other.attachedRigidbody.GetComponent<PlayerTreeAttackObject>();

            if (playerTreeAttackObject != null)
            {
                Debug.Log("3333");
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

            Debug.Log("Player TimeZone ?΄μ₯");

            if (player != null)
            {
                player.AdjustObjectSpeed(1f / speedMultiplier);
                player.isInTimeZone = false;
            }
        }
        else if (other.CompareTag("F_Tree")) // FallingTree??κ²½μ°
        {
            FallingTree fallingTree = other.GetComponent<FallingTree>();

            Debug.Log("FallingTree TimeZone ?΄μ₯");

            if (fallingTree != null)
            {
                fallingTree.AdjustFallSpeed(1f / speedMultiplier);
                fallingTree.ExitTimeZone();
            }
        }
        // GrowingTree??κ²½μ°
        else if (other.CompareTag("GrowingTree")&& CompareTag("FastTimeZone"))
        {
            GrowingTree growingTree = other.GetComponent<GrowingTree>();

            Debug.Log("GrowingTree TimeZone ?΄μ₯");

            if (growingTree != null)
            {
                growingTree.ExitFastTimeZone();
            }
        }
        else if (other.CompareTag("MovingPlatform"))
        {
            MovingTilemap movingPlatform = other.GetComponent<MovingTilemap>();

            Debug.Log("MovingPlatform TimeZone ?΄μ₯");

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

        if (other.CompareTag("PlayerTreeAttackObjects"))
        {
            PlayerTreeAttackObject playerTreeAttackObject = other.GetComponent<PlayerTreeAttackObject>();

            if (playerTreeAttackObject != null)
            {
                playerTreeAttackObject.AdjustObjectSpeed(1f / speedMultiplier);
                playerTreeAttackObject.isInTimeZone = false;
            }
        }
    }
    
}
