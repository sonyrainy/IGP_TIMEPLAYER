using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTreeAttackObject : TimeZoneObject
{
    [SerializeField] GameObject[] colliders;

    public bool isInTimeZone = false;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInTimeZone == true)
        {
            animator.speed = 1f;
        }
        else
        {
            animator.speed = 0.01f;
        }
    }

    public void ChangeCollider(int index)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].SetActive(false);
        }

        colliders[index].SetActive(true);
    }
}
