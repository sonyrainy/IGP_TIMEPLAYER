using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Forest_Boss_States
{
    [System.Serializable]
    public enum ForestBossState
    {
        Spawn, Idle, damaged, RockAttack, LogAttack, Hit, Dead, Last
    }

    public enum ForestBossAnimation
    {
        ForestBoss_Spawn,
        ForestBoss_Idle,
        ForestBoss_Damaged,
        ForestBoss_RockAttack,
        ForestBoss_LogAttack,
        ForestBoss_Hit,
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
            if (user.isDead)
            {
                user.ChangeState(ForestBossState.Dead);
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
                AnimatorStateInfo stateInfo = user.animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("ForestBoss_LogAttack") && stateInfo.normalizedTime >= 1.0f)
                {
                    isAnimationComplete = true;
                }
            }
            
            if (isAnimationComplete == true)
            {
                user.InstantiateLogs();
                isExit = true;
            }
        }

        public override void Exit()
        {

        }

        public override void OnTransition()
        {
            if (user.isDead)
            {
                user.ChangeState(ForestBossState.Dead);
            }
            else
            {
                if (isExit)
                {
                    user.ChangeState(ForestBossState.Idle);
                }
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
                AnimatorStateInfo stateInfo = user.animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("ForestBoss_RockAttack") && stateInfo.normalizedTime >= 1.0f)
                {
                    isAnimationComplete = true;
                }
            }

            if (isAnimationComplete == true)
            {
                user.InstantiateRocks();
                isExit = true;
            }
        }

        public override void Exit()
        {
            
        }

        public override void OnTransition()
        {
            if (user.isDead)
            {
                user.ChangeState(ForestBossState.Dead);
            }
            else
            {
                if (isExit)
                {
                    user.ChangeState(ForestBossState.Idle);
                }
            }
        }
    }

    public class Hit : State<ForestBoss>
    {        
        private bool isAnimationComplete = false;
        public Hit(ForestBoss user) : base(user) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("F_Boss: Hit State");
            user.TakeDamage(1);
            user.ChangeAnimation(ForestBossAnimation.ForestBoss_Hit);
            isAnimationComplete = false;
        }

        public override void Execute()
        {
            if (!isAnimationComplete)
            {
                AnimatorStateInfo stateInfo = user.animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("ForestBoss_Hit") && stateInfo.normalizedTime >= 1.0f)
                {
                    isAnimationComplete = true;
                }
            }
        }

        public override void Exit()
        {
            
        }

        public override void OnTransition()
        {
            if (user.isDead)
            {
                user.ChangeState(ForestBossState.Dead);
            }
            else
            {
                if (isAnimationComplete)
                {
                    user.ChangeState(ForestBossState.Idle);
                }
            }
        }
    }

    public class Dead : State<ForestBoss>
    {       
        public Dead(ForestBoss user) : base(user) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("F_Boss: Dead State");
            user.ChangeAnimation(ForestBossAnimation.ForestBoss_Dead);
        }

        public override void Execute()
        {

        }

        public override void Exit()
        {
            
        }

        public override void OnTransition()
        {

        }
    }
}
