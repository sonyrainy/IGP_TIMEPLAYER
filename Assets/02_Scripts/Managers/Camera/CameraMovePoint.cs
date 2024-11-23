using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovePoint : MonoBehaviour
{
    public Transform targetPoint; // 카메라가 이동할 목표 지점

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CameraManager cameraManager = FindObjectOfType<CameraManager>();
            if (cameraManager != null && targetPoint != null)
            {
                cameraManager.MoveCameraTo(targetPoint.position);
            }
        }
    }
}