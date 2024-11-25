using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Vector3 target;
    public bool isStopped = false;
    private List<Transform> passengers = new List<Transform>();

    void Start()
    {
        target = pointB.position;
    }

    void Update()
    {
        if (!isStopped)
        {
            // 타일맵 오브젝트를 목표 지점으로 이동
            Vector3 oldPosition = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            Vector3 movement = transform.position - oldPosition;

            // 승객들도 같이 이동
            foreach (Transform passenger in passengers)
            {
                passenger.position += movement;
            }

            // 목표 지점에 도달했으면 반대 지점으로 전환
            if (Vector3.Distance(transform.position, target) < 0.01f)
            {
                target = (target == pointA.position) ? pointB.position : pointA.position;
            }
        }
    }

    public void SetStopped(bool stopped)
    {
        isStopped = stopped;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            passengers.Add(collision.transform); // 플레이어를 승객 리스트에 추가
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            passengers.Remove(collision.transform); // 플레이어를 승객 리스트에서 제거
        }
    }

    public void StopPlatformForDuration(float duration)
    {
        StartCoroutine(StopPlatformCoroutine(duration));
    }

    private IEnumerator StopPlatformCoroutine(float duration)
    {
        SetStopped(true);
        yield return new WaitForSeconds(duration);
        SetStopped(false);
    }
}
