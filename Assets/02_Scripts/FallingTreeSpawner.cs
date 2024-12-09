using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTreeSpawner : MonoBehaviour
{
    public GameObject treePrefab;
    public Transform spawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 플레이어가 특정 위치에 도달하면 나무를 생성하고 떨어뜨림
            GameObject tree = Instantiate(treePrefab, spawnPoint.position, Quaternion.identity);
            FallingTree_ForSpawn fallingTree = tree.AddComponent<FallingTree_ForSpawn>();

        }
    }
}
