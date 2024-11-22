using Forest_Boss_States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestBoss : MonoBehaviour
{
    public ForestBossState forestBossState = ForestBossState.Idle;
    public ForestBossState prevForestBossState = ForestBossState.Idle;
    public Animator animator;
    public State<ForestBoss>[] states;
    public void ChangeState(ForestBossState state) 
    {
        states[(int)forestBossState].Exit();
        prevForestBossState = forestBossState;
        forestBossState = state;
        states[(int)forestBossState].Enter();
    }

    public void ChangeAnimation(ForestBossAnimation newAnimation, float normalizedTime = 0)
    {
        animator.Play(newAnimation.ToString(), 0, normalizedTime);
    }

    public void ChangeAnimation(string animationName, float normalizedTime = 0)
    {
        animator.Play(animationName, 0, normalizedTime);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
