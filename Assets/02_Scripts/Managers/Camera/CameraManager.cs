using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform player;
    public List<Transform> cameraMovePoints;
    public float cameraMoveSpeed = 2.0f; 
    public float verticalFollowSpeed = 2.0f;
    public float verticalOffset = 1.3f; 
    private Transform currentTargetPoint; 
    private Coroutine moveCameraCoroutine; 

    void Update()
    {
        // x축_플레이어와 가장 가까운 CameraMovePoint를 찾아 이동
        Transform closestPoint = FindClosestCameraMovePoint();

        if (closestPoint != null && closestPoint != currentTargetPoint)
        {
            currentTargetPoint = closestPoint;

            if (moveCameraCoroutine != null)
            {
                StopCoroutine(moveCameraCoroutine);
            }

            moveCameraCoroutine = StartCoroutine(MoveCameraToPoint(currentTargetPoint.position.x));
        }

        // y_follow Player
        FollowPlayerVertically();
    }

    private Transform FindClosestCameraMovePoint()
    {
        float minDistance = float.MaxValue;
        Transform closestPoint = null;

        foreach (Transform point in cameraMovePoints)
        {
            float distance = Vector2.Distance(new Vector2(player.position.x, 0), new Vector2(point.position.x, 0));
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPoint = point;
            }
        }

        return closestPoint;
    }

    private IEnumerator MoveCameraToPoint(float targetXPosition)
    {
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            while (Mathf.Abs(mainCamera.transform.position.x - targetXPosition) > 0.1f)
            {
                Vector3 currentPosition = mainCamera.transform.position;
                currentPosition.x = Mathf.Lerp(currentPosition.x, targetXPosition, cameraMoveSpeed * Time.deltaTime);
                mainCamera.transform.position = currentPosition;

                if (currentTargetPoint.position.x != targetXPosition)
                {
                    yield break;
                }

                yield return null;
            }

            mainCamera.transform.position = new Vector3(targetXPosition, mainCamera.transform.position.y, mainCamera.transform.position.z);
        }
    }

    private void FollowPlayerVertically()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            Vector3 currentPosition = mainCamera.transform.position;

            float targetY = player.position.y + verticalOffset;

            currentPosition.y = Mathf.Lerp(currentPosition.y, targetY, verticalFollowSpeed * Time.deltaTime);
            mainCamera.transform.position = currentPosition;
        }
    }
}
