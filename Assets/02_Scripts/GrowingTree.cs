using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TimeEffect에서 관리
public class GrowingTree : MonoBehaviour
{
    public GameObject[] growthStages; 
    public float stayDuration = 2f;  
    private float timeInFastZone = 0f;
    private bool isInFastZone = false;
    private int currentGrowthStage = 0; 

    private void Update()
    {
        if (isInFastZone)
        {
            Debug.Log("FastTimeZone...");
            // FastTimeZone 내부에 있는 경우 시간 증가
            timeInFastZone += Time.deltaTime;

            // 시간이 설정된 지속 시간 이상이면 성장 단계 변경
            if (timeInFastZone >= stayDuration)
            {
                Debug.Log("next level");
                ChangeGrowthStage();
                timeInFastZone = 0f;
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

            currentGrowthStage++;

            // 현재 오브젝트를 파괴하고 새로운 프리팹 생성
            GameObject newTree = Instantiate(growthStages[currentGrowthStage], transform.position, transform.rotation);
            newTree.transform.localScale = transform.localScale;

            // 기존 오브젝트 제거
            Destroy(gameObject);
        }
    }
}
