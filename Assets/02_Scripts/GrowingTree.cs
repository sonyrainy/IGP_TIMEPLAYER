using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingTree : MonoBehaviour
{
    public GameObject[] growthStages;  // 성장 단계에 해당하는 프리팹들
    public float stayDuration = 2f;  // FastTimeZone 내부에 있어야 하는 시간
    private float timeInFastZone = 0f; // FastTimeZone 내부에 머문 시간
    private bool isInFastZone = false; // FastTimeZone 내부에 있는지 여부
    private int currentGrowthStage = 0; // 현재 성장 단계

    private void Update()
    {
        if (isInFastZone)
        {
            Debug.Log("FastTimeZone 내부에 머무는 중... 시간 증가 중.");
            // FastTimeZone 내부에 있는 경우 시간 증가
            timeInFastZone += Time.deltaTime;

            // 시간이 설정된 지속 시간 이상이면 성장 단계 변경
            if (timeInFastZone >= stayDuration)
            {
                Debug.Log("성장 단계 변경 조건 충족. 성장 단계 변경.");
                ChangeGrowthStage();
                timeInFastZone = 0f; // 시간 초기화
            }
        }
        else
        {
            // FastTimeZone에서 벗어나면 시간 초기화
            timeInFastZone = 0f;
        }
    }

    public void EnterFastTimeZone()
    {
        Debug.Log("GrowingTree: FastTimeZone에 진입했습니다.");
        isInFastZone = true;
    }

    public void ExitFastTimeZone()
    {
        Debug.Log("GrowingTree: FastTimeZone에서 나갔습니다.");
        isInFastZone = false;
    }

    private void ChangeGrowthStage()
    {
        if (currentGrowthStage < growthStages.Length - 1)
        {
            Debug.Log("나무 성장 단계 변경: " + (currentGrowthStage + 1));
            // 현재 오브젝트를 성장 단계 프리팹으로 교체
            currentGrowthStage++;

            // 현재 오브젝트를 파괴하고 새로운 프리팹 생성
            GameObject newTree = Instantiate(growthStages[currentGrowthStage], transform.position, transform.rotation);
            newTree.transform.localScale = transform.localScale; // 기존 스케일 유지

            // 기존 오브젝트 제거
            Destroy(gameObject);
        }
    }
}
