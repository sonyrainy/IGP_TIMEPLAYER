using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeZoneManager : MonoBehaviour
{
    public GameObject slowTimeZonePrefab; 
    public GameObject fastTimeZonePrefab;

    private GameObject currentTimeZone;

    // ?�?�존 ?�성 ?�태
    private bool isCreatingTimeZone = false;
    private TimeZoneEffect currentEffect;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // ???�환 ?�에?????�브?�트가 ?��??�도�??�정
    }

    void Update()
    {
        HandleTimeZoneCreation();
    }

    private void HandleTimeZoneCreation()
    {
        // Slow TimeZone, 좌클�?
        if (Input.GetMouseButtonDown(0) && !isCreatingTimeZone)
        {
            CreateTimeZone(slowTimeZonePrefab);
            isCreatingTimeZone = true;
        }
        // Fast TimeZone, ?�클�?
        else if (Input.GetMouseButtonDown(1) && !isCreatingTimeZone)
        {
            CreateTimeZone(fastTimeZonePrefab); 
            isCreatingTimeZone = true;
        }

        // ?�릭 ?�제
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

    private void CreateTimeZone(GameObject timeZonePrefab)
    {
        // timeZonePrefab 존재 ?�인
        if (timeZonePrefab == null)
        {
            Debug.LogError("TimeZone prefab is null. Assign the prefab in the Inspector.");
            return;
        }

        // 마우???�치??TimeZone ?�성
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        currentTimeZone = Instantiate(timeZonePrefab, mousePos, Quaternion.identity);

        // TimeZoneEffect 컴포?�트가 존재?�는지 ?�인
        currentEffect = currentTimeZone.GetComponent<TimeZoneEffect>();
        if (currentEffect != null)
        {
            // currentEffect.speedMultiplier = speedMultiplier;
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
