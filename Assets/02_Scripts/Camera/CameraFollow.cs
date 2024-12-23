using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    public float yPosition;

    private void Update() {
        if (target != null)  {
        //플레이어의 위치와 연동
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y + yPosition, -10);
    }
    }
}
