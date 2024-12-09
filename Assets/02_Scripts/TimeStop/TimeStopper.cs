using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopper : MonoBehaviour
{
    public float stopRadius = 7f;
    public float stopDuration = 1.25f;
    private bool isTimeStopped = false;

    public bool IsTimeStopped()
    {
        return isTimeStopped;
    }
    public IEnumerator StopTime()
    {
        // 반복실행 방지
        if (isTimeStopped) yield break; 
        isTimeStopped = true;

        //Debug.Log("Q 키를 눌러 시간이 정지되었습니다.");

        // 반경 내 콜라이더 찾기
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, stopRadius);

        // 찾은 콜라이더 중 특정 태그를 가진 오브젝트 stop
        List<Rigidbody2D> affectedRigidbodies = new List<Rigidbody2D>();
        foreach (Collider2D collider in colliders)
        {
            // 특정 태그를 가진 오브젝트만 멈추게 합니다.
            if (collider.CompareTag("MovingPlatform") || collider.CompareTag("BossAttackObjects"))
            {
                Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    // Rigidbody의 속도를 저장하고 멈춤
                    affectedRigidbodies.Add(rb);
                    rb.velocity = Vector2.zero;
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;
                }

                // 애니메이터 멈춤 (적 애니메이션이 있다면)
                // Animator animator = collider.GetComponent<Animator>();
                // if (animator != null)
                // {
                //     animator.enabled = false;
                // }

                // 움직이는 발판 및 나무 멈춤 (특정 태그로 구분)
                if (collider.CompareTag("MovingPlatform"))
                {
                    MovingTilemap platform = collider.GetComponent<MovingTilemap>();
                    if (platform != null)
                    {
                        platform.StopPlatformForDuration(stopDuration);
                    }
                }


                if (collider.CompareTag("BossAttackObjects"))
                {
                    LogObject bossTree = collider.GetComponent<LogObject>();
                    RockObject bossRock = collider.GetComponent<RockObject>();
                    if (bossTree != null)
                    {

                        bossTree.StopMovement(stopDuration);

                    }
                    if (bossRock != null)
                    {

                        bossRock.StopMovement(stopDuration);

                    }
                }
            }
        }

        yield return new WaitForSeconds(stopDuration);
        isTimeStopped = false;


        // 정지된 상태 해제
        foreach (Rigidbody2D rb in affectedRigidbodies)
        {
            rb.constraints = RigidbodyConstraints2D.None;
        }

        foreach (Collider2D collider in colliders)
        {
            // 활성화
            if ( collider.CompareTag("MovingPlatform") || collider.CompareTag("BossAttackObjects"))
            {
                Animator animator = collider.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.enabled = true;
                }



            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stopRadius);
    }

    // 타임스톱 이펙트
    public IEnumerator ActivateTimeStopEffect(GameObject timeStopEffect)
    {
        timeStopEffect.SetActive(true);
        yield return new WaitForSeconds(1.25f);
        timeStopEffect.SetActive(false);
    }
}
