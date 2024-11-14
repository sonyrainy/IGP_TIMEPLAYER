using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeZoneManager : MonoBehaviour
{
    public GameObject slowTimeZonePrefab; 
    public GameObject fastTimeZonePrefab;

    private GameObject currentTimeZone;

    // 타임존 생성 상태
    private bool isCreatingTimeZone = false;
    private TimeZoneEffect currentEffect;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // 씬 전환 후에도 이 오브젝트가 유지되도록 설정
    }

    void Update()
    {
        HandleTimeZoneCreation();
    }

    private void HandleTimeZoneCreation()
    {
        // Slow TimeZone, 좌클릭
        if (Input.GetMouseButtonDown(0) && !isCreatingTimeZone)
        {
            CreateTimeZone(slowTimeZonePrefab, 0.5f);
            isCreatingTimeZone = true;
        }
        // Fast TimeZone, 우클릭
        else if (Input.GetMouseButtonDown(1) && !isCreatingTimeZone)
        {
            CreateTimeZone(fastTimeZonePrefab, 2.0f); 
            isCreatingTimeZone = true;
        }

        // 클릭 해제
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            DestroyCurrentTimeZone();
            isCreatingTimeZone = false;
        }

        if (isCreatingTimeZone)
        {
            UpdateTimeZonePosition();
        }
    }

    private void CreateTimeZone(GameObject timeZonePrefab, float speedMultiplier)
    {
        // timeZonePrefab이 null인지 확인
        if (timeZonePrefab == null)
        {
            Debug.LogError("TimeZone prefab is null. Assign the prefab in the Inspector.");
            return;
        }

        // 마우스 위치에 TimeZone 생성
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        currentTimeZone = Instantiate(timeZonePrefab, mousePos, Quaternion.identity);

        // TimeZoneEffect 컴포넌트가 존재하는지 확인
        currentEffect = currentTimeZone.GetComponent<TimeZoneEffect>();
        if (currentEffect != null)
        {
            currentEffect.speedMultiplier = speedMultiplier;
        }
        else
        {
            Debug.LogError("TimeZone prefab does not contain a TimeZoneEffect component.");
        }
    }

    private void UpdateTimeZonePosition()
    {
        if (currentTimeZone != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            currentTimeZone.transform.position = mousePos;
        }
    }

    private void DestroyCurrentTimeZone()
    {
        if (currentTimeZone != null)
        {
            Destroy(currentTimeZone);
            currentTimeZone = null;
        }
    }
}
