using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeZoneEffect : MonoBehaviour
{
    // Timezone ?�어갔을 ???�도 ?�만??변?�게 ?�도�??��? 배율
    // ?�?�존마다 prefab?�서 ?�르�??�정?�어 ?�음.
    public float speedMultiplier = 1f;

    public void setSpeedMultiplier (float speedMultiplier = 1.0f)
    {
        this.speedMultiplier = speedMultiplier;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TimeZoneObject timeZoneObject = other.attachedRigidbody.GetComponent<TimeZoneObject>();

        if (timeZoneObject != null)
        {
            Debug.Log("TimeZoneObject: Enter the TimeZone ");
            timeZoneObject.AdjustObjectSpeed(speedMultiplier);
            timeZoneObject.isInTimeZone = true;
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

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        TimeZoneObject timeZoneObject = other.attachedRigidbody.GetComponent<TimeZoneObject>();

        if (timeZoneObject != null)
        {
            Debug.Log("TimeZoneObject: Exit the TimeZone");
            timeZoneObject.AdjustObjectSpeed(1f / speedMultiplier);
            timeZoneObject.isInTimeZone = false;
        }
        
        if (other.CompareTag("F_Tree")) // FallingTree??경우
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

    }
    
}
