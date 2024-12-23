using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovePoint : MonoBehaviour
{
    // 카메라가 이동할 지점
    public Transform targetPoint; 
    private bool hasTriggered = false; // 트리거가 이미 발생했는지 여부

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Player") || collision.CompareTag("CameraMove")) && !hasTriggered)
        {
            CameraManager cameraManager = FindObjectOfType<CameraManager>();

            if (cameraManager != null && targetPoint != null)
            {
                // 카메라가 targetPoint의 x 성분에 맞게 이동하도록 설정
                cameraManager.MoveCameraTo(new Vector3(targetPoint.position.x, cameraManager.transform.position.y, cameraManager.transform.position.z));
            }
            
            // 트리거가 발생했음을 기록하고, 스폰 포인트 업데이트
            PlayerRespawnManager.Instance.UpdateSpawnPointIndex(1);
            
            // 트리거가 발생했음을 기록하고 isTrigger 비활성화
            hasTriggered = true;
            StartCoroutine(DisableTriggerAfterDelay(0.5f)); // 약간의 지연 후 isTrigger 비활성화
        }
    }

    private IEnumerator DisableTriggerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GetComponent<Collider2D>().isTrigger = false;
    }
}
