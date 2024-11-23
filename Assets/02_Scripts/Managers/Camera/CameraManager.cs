using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraManager : MonoBehaviour
{
    private GameObject mainCamera;
    private Vector3 destination;
    private Vector3 originalPosition;
    private bool moveCamera = false;
    private float cameraSpeed = 45.0f;

    private void Start()
    {
        mainCamera = Camera.main.gameObject;
        originalPosition = mainCamera.transform.position;
        destination = originalPosition;
    }

    private void Update()
    {
        if (moveCamera)
        {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, destination, cameraSpeed * Time.deltaTime);
            if (mainCamera.transform.position == destination)
            {
                moveCamera = false;
                StartCoroutine(CameraShake(0.1f, 0.2f));
            }
        }
    }

    public void MoveCameraTo(Vector3 targetPosition)
    {
        if (!moveCamera)
        {
            if (mainCamera.transform.position == originalPosition)
            {
                destination = new Vector3(targetPosition.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
            }
            else
            {
                destination = originalPosition;
            }
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
