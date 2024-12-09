using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform playerTransform; // 플레이어의 Transform
    private GameObject mainCamera;
    private Vector3 destination;
        private Vector3 originalPosition;

    private bool moveCamera = false;
    private float cameraSpeed = 45.0f;
    public float yOffset = 2.0f; // 카메라가 플레이어보다 얼마나 위에 위치할지 설정

    private void Start()
    {
        mainCamera = Camera.main.gameObject;
                originalPosition = mainCamera.transform.position;

        destination = originalPosition;
    }

    private void Update()
    {
        // 카메라의 y축을 플레이어보다 살짝 위에 맞춰 자연스럽게 이동
        Vector3 currentPosition = mainCamera.transform.position;
        currentPosition.y = Mathf.Lerp(currentPosition.y, playerTransform.position.y + yOffset, Time.deltaTime * cameraSpeed * 0.1f);
        mainCamera.transform.position = currentPosition;

        // 카메라가 목표 지점으로 x축 이동
        if (moveCamera)
        {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, destination, cameraSpeed * Time.deltaTime);
            if (Mathf.Abs(mainCamera.transform.position.x - destination.x) < 0.01f)
            {
                // x축 목표에 도달했을 때 이동 종료
                mainCamera.transform.position = new Vector3(destination.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
                moveCamera = false;
                StartCoroutine(CameraShake(0.1f, 0.2f));

                
            }
        }
    }

    public void MoveCameraTo(Vector3 targetPosition)
    {
        // 카메라가 이동 중이 아닐 때만 이동 설정
        if (!moveCamera)
        {
            // 목표 지점의 x 위치를 설정하고 y, z는 현재 카메라 위치 유지
            destination = new Vector3(targetPosition.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
            moveCamera = true;
        }
    }

    private IEnumerator CameraShake(float duration, float magnitude)
    {
        Vector3 originalCamPos = mainCamera.transform.position;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            mainCamera.transform.position = new Vector3(originalCamPos.x + offsetX, originalCamPos.y + offsetY, originalCamPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = originalCamPos;
    }
}
