using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeZoneManager : MonoBehaviour
{
    public GameObject slowTimeZonePrefab;   // 느려지는 TimeZone 프리팹
    public GameObject fastTimeZonePrefab;   // 빨라지는 TimeZone 프리팹
    private GameObject currentTimeZone;     // 현재 생성된 TimeZone
    private bool isCreatingTimeZone = false; // TimeZone이 생성 중인지 확인

    void Update()
    {
        HandleTimeZoneCreation();
    }

    private void HandleTimeZoneCreation()
    {
        if (Input.GetMouseButtonDown(0) && !isCreatingTimeZone) // 좌클릭
        {
            CreateTimeZone(slowTimeZonePrefab);
            isCreatingTimeZone = true;
        }
        else if (Input.GetMouseButtonDown(1) && !isCreatingTimeZone) // 우클릭
        {
            CreateTimeZone(fastTimeZonePrefab);
            isCreatingTimeZone = true;
        }

        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            // 클릭을 떼면 TimeZone 파괴
            DestroyCurrentTimeZone();
            isCreatingTimeZone = false;
        }

        if (isCreatingTimeZone)
        {
            // 클릭을 유지하고 있는 동안 마우스 위치를 따라 TimeZone 이동
            UpdateTimeZonePosition();
        }
    }

    private void CreateTimeZone(GameObject timeZonePrefab)
    {
        // 마우스 위치에 TimeZone 생성
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; // 2D이므로 Z값 고정

        currentTimeZone = Instantiate(timeZonePrefab, mousePos, Quaternion.identity);
    }

    private void UpdateTimeZonePosition()
    {
        if (currentTimeZone != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f; // 2D이므로 Z값 고정
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
