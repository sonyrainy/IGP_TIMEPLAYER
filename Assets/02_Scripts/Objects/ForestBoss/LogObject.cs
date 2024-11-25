using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogObject : TimeZoneObject
{
    Transform transform;
    [SerializeField] float yVelocity = 0;
    [SerializeField] float gravity = 9.8f;

    public bool isInTimeZone = false;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        Vector3 position = transform.position;
        position.y -= yVelocity * gravity * Time.deltaTime * speedMultiplier;
        transform.position = position;
    }
}
