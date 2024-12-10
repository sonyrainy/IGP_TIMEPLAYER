using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeZoneObject : MonoBehaviour
{
    public float speedMultiplier = 1f;
    public bool isInTimeZone = false;


    public virtual void AdjustObjectSpeed(float speedMultiplier)
    {
        this.speedMultiplier = speedMultiplier;
    }
}
