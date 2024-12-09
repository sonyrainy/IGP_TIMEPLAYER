using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenTree : MonoBehaviour
{
    public GameObject fallenTreePrefab;  // 쓰러진 나무 프리팹
    public Transform treeTop;  // 나무의 위쪽 부분 (자식 오브젝트)
    public float rotationZMin = 0f;  // 회전 시작값
    public float rotationZMax = -50f;  // 회전 끝값
    public Vector2 positionOffset = new Vector2(1f, -0.5f);  // 이동할 x, y 값의 오프셋
    public float stayDuration = 2f;  // FastTimeZone 내부에 있어야 하는 시간
    public float fallDuration = 2f;  // 나무가 서서히 쓰러지는 시간

    private bool isInFastZone = false; // FastTimeZone 내부에 있는지 여부
    private float timeInFastZone = 0f; // FastTimeZone 내부에 머문 시간
    private bool isBroken = false;  // 나무가 쓰러졌는지 여부

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

        // 회전 시작과 끝 값 설정
        Quaternion startRotation = treeTop.localRotation;
        Quaternion endRotation = Quaternion.Euler(0f, 0f, rotationZMax);

        // 위치 시작과 끝 값 설정
        Vector3 startPosition = treeTop.localPosition;
        Vector3 endPosition = startPosition + (Vector3)positionOffset;

        // 서서히 회전하면서 나무가 쓰러지고, 위치도 이동함
        float elapsed = 0f;
        while (elapsed < fallDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fallDuration;

            // 회전 및 위치 서서히 변경
            treeTop.localRotation = Quaternion.Slerp(startRotation, endRotation, t);
            treeTop.localPosition = Vector3.Lerp(startPosition, endPosition, t);

            yield return null;
        }

        // 쓰러진 나무 프리팹으로 변경
        GameObject fallenTree = Instantiate(fallenTreePrefab, transform.position, transform.rotation);
        //fallenTree.transform.localScale = transform.localScale; // 기존 스케일 유지

        // 현재 나무 오브젝트 제거
        Destroy(gameObject);
    }
}
