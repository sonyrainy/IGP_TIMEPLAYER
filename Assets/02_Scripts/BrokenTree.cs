using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenTree : MonoBehaviour
{
    public GameObject fallenTreePrefab; 
    public Transform treeTop;
    public float rotationZMin = 0f; 
    public float rotationZMax = -50f;
    public Vector2 positionOffset = new Vector2(1f, -0.5f); 
    public float stayDuration = 2f;
    public float fallDuration = 2f;

    private bool isInFastZone = false;
    private float timeInFastZone = 0f;
    private bool isBroken = false;

    void Update()
    {
        if (isInFastZone && !isBroken)
        {
            timeInFastZone += Time.deltaTime;

            if (timeInFastZone >= stayDuration)
            {
                StartCoroutine(SlowBreakTree());
                timeInFastZone = 0f;
            }
        }
        else
        {
            timeInFastZone = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FastTimeZone"))
        {
            isInFastZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("FastTimeZone"))
        {
            isInFastZone = false;
        }
    }

    private IEnumerator SlowBreakTree()
    {
        if (isBroken) yield break;

        isBroken = true;

        Quaternion startRotation = treeTop.localRotation;
        Quaternion endRotation = Quaternion.Euler(0f, 0f, rotationZMax);

        Vector3 startPosition = treeTop.localPosition;
        Vector3 endPosition = startPosition + (Vector3)positionOffset;

        float elapsed = 0f;
        while (elapsed < fallDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fallDuration;

            treeTop.localRotation = Quaternion.Slerp(startRotation, endRotation, t);
            treeTop.localPosition = Vector3.Lerp(startPosition, endPosition, t);

            yield return null;
        }

        // 쓰러진 나무 프리팹으로 변경
        GameObject fallenTree = Instantiate(fallenTreePrefab, transform.position, transform.rotation);

        // 현재 나무 오브젝트 제거
        Destroy(gameObject);
    }
}
