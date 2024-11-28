using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTree_ForSpawn : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // 플레이어와 충돌 시 현재 리스폰 포인트로 이동
            Player player = collision.collider.GetComponent<Player>();
            if (player != null)
            {
                player.RespawnAtLastSpawnPoint();
            }
        }
        else if (collision.collider.CompareTag("Ground"))
        {
            // 땅과 충돌 시 나무 삭제
            Destroy(gameObject); // 나무 오브젝트를 삭제
        }
    }
}
