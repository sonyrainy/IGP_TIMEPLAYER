using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTree_ForSpawn : TimeZoneObject
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // ?�레?�어?� 충돌 ???�재 리스???�인?�로 ?�동
            Player player = collision.collider.GetComponent<Player>();
            if (player != null)
            {
                player.RespawnAtLastSpawnPoint();
            }
        }
        else if (collision.collider.CompareTag("Ground"))
        {
            // ?�과 충돌 ???�무 ??��
            Destroy(gameObject); // ?�무 ?�브?�트�???��
        }
    }
}
