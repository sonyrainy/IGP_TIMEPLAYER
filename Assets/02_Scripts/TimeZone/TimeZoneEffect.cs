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
                player.AdjustObjectSpeed(speedMultiplier);
                player.isInTimeZone = true;
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

            Debug.Log("Player TimeZone 퇴장"); 

            if (player != null)
            {
                player.AdjustObjectSpeed(1f / speedMultiplier);
                player.isInTimeZone = false;
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
