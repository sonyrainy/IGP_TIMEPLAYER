﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockObject : TimeZoneObject
{
    public float xVelocity = 0;
    public float acceleration = 0.5f; // 가속도
    public float destroyXPosition = -10f; // 파괴될 X축 위치

    private float originalXVelocity; // 원래의 xVelocity 값을 저장
    private float originalAcceleration; // 원래의 가속도 값을 저장

    private bool isStopped = false; // 타임스톱 상태 플래그

    void Start()
    {
        originalXVelocity = xVelocity;
        originalAcceleration = acceleration; 
    }

    void Update()
    {
        if (!isStopped)
        {
            HorizontalMove();
        }
    }

    private void HorizontalMove()
    {
        xVelocity -= Time.deltaTime * acceleration;
        xVelocity = Mathf.Max(xVelocity, -2);

        Vector3 position = transform.position;
        position.x += xVelocity * Time.deltaTime * speedMultiplier;
        transform.position = position;
    }

    public void StopMovement(float duration)
    {
        if (!isStopped)
        {
            StartCoroutine(StopMovementCoroutine(duration));
        }
    }

    private IEnumerator StopMovementCoroutine(float duration)
    {
        isStopped = true;

        // 속도와 가속도를 0으로 설정하여 정지
        xVelocity = 0;
        acceleration = 0;

        yield return new WaitForSeconds(duration);

        xVelocity = originalXVelocity;
        acceleration = originalAcceleration;

        isStopped = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}