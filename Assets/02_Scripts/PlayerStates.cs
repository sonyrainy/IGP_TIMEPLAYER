using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Player_States
{
    [System.Serializable]
    public enum PlayerState
    {
        Spawn, Idle, Run, Jump, Dash, Last
    }

    public enum PlayerAnimation
    {
        Player_Spawn,
        Player_Idle,
        Player_Run,
        Player_Jump,
        Player_Dash,
        Last
    }

    public class Spawn : State<Player>
    {
        float timer = 0f;
        public Spawn(Player user) : base(user) { }

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
                user.ChangeState(PlayerState.Idle);
            }
        }
    }

    public class Idle : State<Player>
    {
        public Idle(Player user) : base(user) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Idle State");
            user.animator.SetBool("isRun", false);
        }

        public override void Execute()
        {

        }

        public override void Exit()
        {

        }

        public override void OnTransition()
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                user.ChangeState(PlayerState.Run);
            }
        }
    }

    public class Run : State<Player>
    {
        public Run(Player user) : base(user) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Run State");
        }

        public override void Execute()
        {
            user.animator.SetBool("isRun", Input.GetAxisRaw("Horizontal") != 0);

            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                Vector3 scale = user.transform.localScale;
                scale.x = Input.GetAxisRaw("Horizontal");
                user.transform.localScale = scale;
            }

            user.rigidbody.velocity = new Vector2(user.moveSpeed * Input.GetAxisRaw("Horizontal"), user.rigidbody.velocity.y);
        }

        public override void Exit()
        {

        }

        public override void OnTransition()
        {
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                user.ChangeState(PlayerState.Idle);
            }
        }
    }

    public class Jump : State<Player>
    {
        private bool hasJumped;

        public Jump(Player user) : base(user) { }
        
        public override void Enter()
        {
            base.Enter();
            Debug.Log("Jump State");
        }

        public override void Execute()
        {
            if (!hasJumped && user.isGround)
            {
                user.yVelocity = user.jumpForce;  // 점프 시작 시 yVelocity를 jumpForce로 설정
                user.isGround = false;
                user.animator.SetTrigger("Jump");
                hasJumped = true;
            }
        }

        public override void Exit()
        {

        }

        public override void OnTransition()
        {
            if (user.isGround == true)
            {
                user.ChangeState(PlayerState.Idle);
                hasJumped = !hasJumped;
            }
        }
    }

    public class Dash : State<Player>
    {
        public Dash(Player user) : base(user) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Dash State");
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
