using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Time : MonoBehaviour
{
    protected bool isSlow = false;
    protected bool isFast = false;

    public float slowValue = 0.5f;
    public float fastValue = 2.0f;
}
