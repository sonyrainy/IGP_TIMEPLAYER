using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeZoneManager : MonoBehaviour
{
    public GameObject slowTimeZonePrefab; 
    public GameObject fastTimeZonePrefab;

    private GameObject currentTimeZone;

    // ?Ä?ÑÏ°¥ ?ùÏÑ± ?ÅÌÉú
    private bool isCreatingTimeZone = false;
    private TimeZoneEffect currentEffect;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // ???ÑÌôò ?ÑÏóê?????§Î∏å?ùÌä∏Í∞Ä ?†Ï??òÎèÑÎ°??§Ï†ï
    }

    void Update()
    {
        HandleTimeZoneCreation();
    }

    private void HandleTimeZoneCreation()
    {
        // Slow TimeZone, Ï¢åÌÅ¥Î¶?
        if (Input.GetMouseButtonDown(0) && !isCreatingTimeZone)
        {
            CreateTimeZone(slowTimeZonePrefab, 0.5f);
            isCreatingTimeZone = true;
        }
        // Fast TimeZone, ?∞ÌÅ¥Î¶?
        else if (Input.GetMouseButtonDown(1) && !isCreatingTimeZone)
        {
            CreateTimeZone(fastTimeZonePrefab, 2.0f); 
            isCreatingTimeZone = true;
        }

        // ?¥Î¶≠ ?¥Ï†ú
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
        // timeZonePrefab Ï°¥Ïû¨ ?ïÏù∏
        if (timeZonePrefab == null)
        {
            Debug.LogError("TimeZone prefab is null. Assign the prefab in the Inspector.");
            return;
        }

        // ÎßàÏö∞???ÑÏπò??TimeZone ?ùÏÑ±
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        currentTimeZone = Instantiate(timeZonePrefab, mousePos, Quaternion.identity);

        // TimeZoneEffect Ïª¥Ìè¨?åÌä∏Í∞Ä Ï°¥Ïû¨?òÎäîÏßÄ ?ïÏù∏
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
