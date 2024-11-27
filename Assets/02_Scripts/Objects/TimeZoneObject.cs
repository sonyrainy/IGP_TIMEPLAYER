using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeZoneObject : MonoBehaviour
{
    public float speedMultiplier = 1f;

    public void AdjustObjectSpeed(float speedMultiplier)
    {
        this.speedMultiplier = speedMultiplier;
    }
}
