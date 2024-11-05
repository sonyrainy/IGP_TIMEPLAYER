using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeZoneManager : Time
{
    public GameObject slowTimeZonePrefab; 
    public GameObject fastTimeZonePrefab;

    private GameObject currentTimeZone;

    //타임존 생성되어있는지
    private bool isCreatingTimeZone = false;
    private TimeZoneEffect currentEffect;

    void Update()
    {
        HandleTimeZoneCreation();
    }

    private void HandleTimeZoneCreation()
    {

        if (!isCreatingTimeZone)
        {
            isSlow = false;
            isFast = false;
        }

        //Slow TimeZone, 좌클릭
        if (Input.GetMouseButtonDown(0) && !isCreatingTimeZone)
        {
            CreateTimeZone(slowTimeZonePrefab, slowValue);
            isCreatingTimeZone = true;
            isSlow = true;
            isFast = false;
        }

        //Fast TimeZone, 우클릭
        else if (Input.GetMouseButtonDown(1) && !isCreatingTimeZone)
        {
            CreateTimeZone(fastTimeZonePrefab, fastValue); 
            isCreatingTimeZone = true;
            isSlow = false;
            isFast = true;
        }

        //동시 클릭 X
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
        // 마우스 위치에 TimeZone 생성
        //+) ScreenToWorldPoint를 통해
        // 마우스 포인터 화면 좌표 월드 좌표로 변환하여 생성하도록 하기 위해서
        // 만들었다네요 ^-^
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        currentTimeZone = Instantiate(timeZonePrefab, mousePos, Quaternion.identity);
        currentEffect = currentTimeZone.AddComponent<TimeZoneEffect>();
        currentEffect.speedMultiplier = speedMultiplier;
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
