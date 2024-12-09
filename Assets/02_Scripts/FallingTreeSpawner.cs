using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTreeSpawner : MonoBehaviour
{
    public GameObject treePrefab; // 나무 프리팹
    public Transform spawnPoint; // 나무가 생성될 위치
    public float fallSpeed = 5f; // 나무의 기본 낙하 속도
    public LayerMask slowTimeZoneLayer; // SlowTimeZone 레이어 설정

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 플레이어가 특정 위치에 도달하면 나무를 생성하고 떨어뜨림
            GameObject tree = Instantiate(treePrefab, spawnPoint.position, Quaternion.identity);
            FallingTree fallingTree = tree.AddComponent<FallingTree>();
            fallingTree.SetFallSpeed(fallSpeed);
            //fallingTree.SetSlowTimeLayer(slowTimeZoneLayer);
        }
    }
}
