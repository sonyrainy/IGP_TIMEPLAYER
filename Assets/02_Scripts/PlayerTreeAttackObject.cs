using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTreeAttackObject : TimeZoneObject
{
    [SerializeField] GameObject[] colliders;
    [SerializeField] ForestBoss forestBoss;

    public bool isInTimeZone = false;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        forestBoss = GetComponent<ForestBoss>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInTimeZone == true)
        {
            animator.speed = speedMultiplier;
        }
        else
        {
            animator.speed = 0.01f;
        }

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Grow") && stateInfo.normalizedTime >= 1.0f)
        {
            forestBoss.isHit = true;
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
