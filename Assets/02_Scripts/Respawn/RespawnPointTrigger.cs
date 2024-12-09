using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPointTrigger : MonoBehaviour
{
   private int indexIncrement = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerRespawnManager respawnManager = PlayerRespawnManager.Instance;

            if (respawnManager != null)
            {
                respawnManager.UpdateSpawnPointIndex(indexIncrement);

                Debug.Log($"Player entered trigger. Respawn Point Index increased by {indexIncrement}. Current Index: {respawnManager.GetCurrentSpawnPoint().name}");
            }

                        gameObject.SetActive(false);

        }
    }
}
