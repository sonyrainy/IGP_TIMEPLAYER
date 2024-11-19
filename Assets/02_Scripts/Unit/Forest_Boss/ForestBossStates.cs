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

    public class Spawn : State<ForestBoss>
    {
        float timer = 0f;
        public Spawn(ForestBoss boss) : base(boss) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Spawn State");
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
            if (timer > 0.5f)
            {
                user.ChangeState(ForestBossState.Idle);
            }
        }
    }
}
