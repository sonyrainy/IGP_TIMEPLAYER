using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnManager : MonoBehaviour
{
    public Transform[] playerCheckpoints; // 플레이어가 재생성될 체크포인트들
    public static PlayerRespawnManager Instance { get; private set; }

    private int currentCheckpointIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateSpawnPoint(Transform newSpawnPoint)
    {
        for (int i = 0; i < playerCheckpoints.Length; i++)
        {
            if (playerCheckpoints[i] == newSpawnPoint)
            {
                currentCheckpointIndex = i;
                break;
            }
        }
    }

    public Transform GetCurrentSpawnPoint()
    {
        Debug.Log("Current Spawn Point: " + playerCheckpoints[currentCheckpointIndex].name);

        return playerCheckpoints[currentCheckpointIndex];
    }
    
    internal void UpdateSpawnPointIndex(int increment)
    {
        currentCheckpointIndex = Mathf.Clamp(currentCheckpointIndex + increment, 0, playerCheckpoints.Length - 1);
    }
}
