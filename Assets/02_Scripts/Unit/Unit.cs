using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public bool isInTimeZone = false;
    public float inTimeZoneSpeed = 1;

    public abstract void OnDie();

    public abstract void ApplyGravity();
}
