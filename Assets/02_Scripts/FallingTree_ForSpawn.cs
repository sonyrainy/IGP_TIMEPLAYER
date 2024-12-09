using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTree_ForSpawn : TimeZoneObject
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // ?Œë ˆ?´ì–´?€ ì¶©ëŒ ???„ì¬ ë¦¬ìŠ¤???¬ì¸?¸ë¡œ ?´ë™
            Player player = collision.collider.GetComponent<Player>();
            if (player != null)
            {
                player.RespawnAtLastSpawnPoint();
            }
        }
        else if (collision.collider.CompareTag("Ground"))
        {
            // ?…ê³¼ ì¶©ëŒ ???˜ë¬´ ?? œ
            Destroy(gameObject); // ?˜ë¬´ ?¤ë¸Œ?íŠ¸ë¥??? œ
        }
    }
}
