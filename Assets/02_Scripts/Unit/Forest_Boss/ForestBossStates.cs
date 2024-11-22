using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Forest_Boss_States
{
    [System.Serializable]
    public enum ForestBossState
    {
        Spawn, Idle, damaged, RockAttack, LogAttack, Dead, Last
    }

    public enum ForestBossAnimation
    {
        ForestBoss_Spawn,
        ForestBoss_Idle,
        ForestBoss_Damaged,
        ForestBoss_RockAttack,
        ForestBoss_LogAttack,
        ForestBoss_Dead,
        Last
    }

    public class FBossSpawn : State<ForestBoss>
    {
        float timer = 0f;
        public FBossSpawn(ForestBoss user) : base(user) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("F_Boss: Spawn State");
            user.ChangeAnimation(ForestBossAnimation.ForestBoss_Spawn);
            timer = 0f;
        }

        public override void Execute()
        {
            timer += Time.deltaTime;
        }

        public override void Exit() 
        {
            
        }

        public override void OnTransition()
        {
            if (timer > 4f)
            {
                user.ChangeState(ForestBossState.Idle);
            }
        }
    }
    public class FBossIdle : State<ForestBoss>
    {
        float timer = 0f;
        public FBossIdle(ForestBoss user) : base(user) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("F_Boss: Idle State");
            user.ChangeAnimation(ForestBossAnimation.ForestBoss_Idle);
        }

        public override void Execute()
        {
            timer += Time.deltaTime;
        }
        public override void Exit()
        {

        }

        public override void OnTransition()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                user.ChangeState(ForestBossState.LogAttack);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                user.ChangeState(ForestBossState.RockAttack);
            }
        }
    }

    public class LogAttack : State<ForestBoss>
    {
        private bool isAnimationComplete = false;
        private bool isExit = false;

        public LogAttack(ForestBoss user) : base(user) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("F_Boss: LogAttack State");
            user.ChangeAnimation(ForestBossAnimation.ForestBoss_LogAttack);
            isAnimationComplete = false;
            isExit = false;
        }

        public override void Execute()
        {
            if (!isAnimationComplete)
            {
                // 애니메이션 상태 확인
                AnimatorStateInfo stateInfo = user.animator.GetCurrentAnimatorStateInfo(0); // 0은 기본 레이어
                if (stateInfo.IsName("ForestBoss_LogAttack") && stateInfo.normalizedTime >= 1.0f)
                {
                    isAnimationComplete = true;
                }
            }
            
            if (isAnimationComplete == true)
            {
                user.InstantiateLogs(); // 애니메이션이 끝난 후 Logs 생성
                isExit = true;
            }
        }

        public override void Exit()
        {

        }

        public override void OnTransition()
        {
            if (isExit)
            {
                user.ChangeState(ForestBossState.Idle);
            }
        }
    }

    public class RockAttack : State<ForestBoss>
    {
        private bool isAnimationComplete = false;
        private bool isExit = false;

        public RockAttack(ForestBoss user) : base(user) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("F_Boss: RockAttack State");
            user.ChangeAnimation(ForestBossAnimation.ForestBoss_RockAttack);
            isAnimationComplete = false;
            isExit = false;
        }

        public override void Execute()
        {
            if (!isAnimationComplete)
            {
                // 애니메이션 상태 확인
                AnimatorStateInfo stateInfo = user.animator.GetCurrentAnimatorStateInfo(0); // 0은 기본 레이어
                if (stateInfo.IsName("ForestBoss_RockAttack") && stateInfo.normalizedTime >= 1.0f)
                {
                    isAnimationComplete = true;
                }
            }

            if (isAnimationComplete == true)
            {
                user.InstantiateRocks(); // 애니메이션이 끝난 후 Rock 생성
                isExit = true;
            }
        }

        public override void Exit()
        {
            
        }

        public override void OnTransition()
        {
            if (isExit)
            {
                user.ChangeState(ForestBossState.Idle);
            }
        }
    }
}
