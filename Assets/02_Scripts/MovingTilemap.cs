using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA; // 시작 지점
    public Transform pointB; // 끝 지점
    public float speed = 2f; // 이동 속도

    private Vector3 target;
    public bool isStopped = false; // 정지 상태를 관리하는 변수
    private List<Transform> passengers = new List<Transform>(); // 플랫폼 위의 승객들

    void Start()
    {
        target = pointB.position; // 초기 목표 지점을 설정
    }

    void Update()
    {
        if (!isStopped) // 정지 상태가 아닐 때만 이동
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

    // 정지 상태를 설정하는 메서드
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
