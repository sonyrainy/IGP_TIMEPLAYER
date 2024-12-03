using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeZoneManager : MonoBehaviour
{
    public GameObject slowTimeZonePrefab; 
    public GameObject fastTimeZonePrefab;

    private GameObject currentTimeZone;

    // ??μ‘΄ ?μ± ?ν
    private bool isCreatingTimeZone = false;
    private TimeZoneEffect currentEffect;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // ???ν ?μ?????€λΈ?νΈκ° ? μ??λλ‘??€μ 
    }

    void Update()
    {
        HandleTimeZoneCreation();
    }

    private void HandleTimeZoneCreation()
    {
        // Slow TimeZone, μ’ν΄λ¦?
        if (Input.GetMouseButtonDown(0) && !isCreatingTimeZone)
        {
            CreateTimeZone(slowTimeZonePrefab, 0.5f);
            isCreatingTimeZone = true;
        }
        // Fast TimeZone, ?°ν΄λ¦?
        else if (Input.GetMouseButtonDown(1) && !isCreatingTimeZone)
        {
            CreateTimeZone(fastTimeZonePrefab, 2.0f); 
            isCreatingTimeZone = true;
        }

        // ?΄λ¦­ ?΄μ 
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
        // timeZonePrefab μ‘΄μ¬ ?μΈ
        if (timeZonePrefab == null)
        {
            Debug.LogError("TimeZone prefab is null. Assign the prefab in the Inspector.");
            return;
        }

        // λ§μ°???μΉ??TimeZone ?μ±
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        currentTimeZone = Instantiate(timeZonePrefab, mousePos, Quaternion.identity);

        // TimeZoneEffect μ»΄ν¬?νΈκ° μ‘΄μ¬?λμ§ ?μΈ
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
