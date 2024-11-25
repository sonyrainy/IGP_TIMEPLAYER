using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopper : MonoBehaviour
{
    public float stopRadius = 5f;  // 시간 정지 반경
    public float stopDuration = 3f; // 시간 정지 지속 시간
    public LayerMask stoppableLayer; // 정지 가능한 오브젝트 레이어
    private bool isTimeStopped = false;

    public IEnumerator StopTime()
    {
        if (isTimeStopped) yield break; // 이미 시간이 정지 중이면 실행하지 않음
        isTimeStopped = true;

        Debug.Log("Q 키를 눌러 시간이 정지되었습니다.");

        // 반경 내의 모든 콜라이더 찾기
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, stopRadius, stoppableLayer);

        // 찾은 콜라이더의 Rigidbody2D 컴포넌트를 멈춤
        List<Rigidbody2D> affectedRigidbodies = new List<Rigidbody2D>();
        foreach (Collider2D collider in colliders)
        {
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Rigidbody의 속도를 저장하고 멈춤
                affectedRigidbodies.Add(rb);
                rb.velocity = Vector2.zero;
                rb.constraints = RigidbodyConstraints2D.FreezeAll; // 모든 움직임 멈춤
            }

            // 애니메이터 멈춤 (만약 적 애니메이션이 있다면)
            Animator animator = collider.GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = false;
            }

            // 움직이는 발판 및 나무 멈춤
            MovingTilemap platform = collider.GetComponent<MovingTilemap>();
            if (platform != null)
            {
                platform.StopPlatformForDuration(stopDuration);
            }

            FallingTree tree = collider.GetComponent<FallingTree>();
            if (tree != null)
            {
                tree.StopPlatformForDuration(stopDuration);
            }
        }

        // 일정 시간 동안 정지 상태 유지
        yield return new WaitForSeconds(stopDuration);

        // 정지된 상태 해제
        foreach (Rigidbody2D rb in affectedRigidbodies)
        {
            rb.constraints = RigidbodyConstraints2D.None; // 모든 제약 해제
        }

        foreach (Collider2D collider in colliders)
        {
            Animator animator = collider.GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = true; // 애니메이터 다시 활성화
            }
        }

        isTimeStopped = false;
    }

    // Gizmos를 사용하여 반경을 시각적으로 표시 (디버깅용)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stopRadius);
    }
}
