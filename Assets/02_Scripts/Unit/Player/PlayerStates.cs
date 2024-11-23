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
            user.ChangeAnimation(PlayerAnimation.Player_Idle);
            // user.animator.SetBool("isRun", false);
        }

        public override void Execute()
        {
            user.rigidbody.velocity = new Vector2(0, 0);
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

            if (Input.GetKeyDown(KeyCode.Space))
            {
                user.ChangeState(PlayerState.Jump);
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                user.ChangeState(PlayerState.Dash);
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
            user.ChangeAnimation("Player_Run");
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

            if (Input.GetKeyDown(KeyCode.Space))
            {
                user.ChangeState(PlayerState.Jump);
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                user.ChangeState(PlayerState.Dash);
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
                user.yVelocity = user.jumpForce;  // ���� ���� �� yVelocity�� jumpForce�� ����
                user.isGround = false;
                //user.animator.SetTrigger("Jump");
                user.ChangeAnimation("Player_Jump");
                hasJumped = true;
            }
        }

        public override void Exit()
        {

        }

        public override void OnTransition()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                user.ChangeState(PlayerState.Dash);
            }

            if (user.isGround == true)
            {
                if (user.rigidbody.velocity.x == 0)
                {
                    user.ChangeState(PlayerState.Idle);
                }
                else
                {
                    user.ChangeState(PlayerState.Run);
                }
                
                hasJumped = !hasJumped;
            }
        }
    }

    public class Dash : State<Player>
    {
        private bool isAnimationComplete = false;
        private bool isExit = false;

        public Dash(Player user) : base(user) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Dash State");
            user.ChangeAnimation(PlayerAnimation.Player_Dash);
            isAnimationComplete = false;
            isExit = false;
        }

        public override void Execute()
        {
            // Vector2 dashVec = new Vector2(user.rigidbody.velocity.x + user.dashFloat, user.rigidbody.velocity.y);

            if (!isAnimationComplete)
            {
                // user.rigidbody.velocity = dashVec;

                user.isDash = true;
                user.yVelocity = 0;

                AnimatorStateInfo stateInfo = user.animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("Player_Dash") && stateInfo.normalizedTime >= 1.0f)
                {
                    isAnimationComplete = true;
                }
            }
            else
            {
                user.isDash = false;
                
                if (user.isGround == false)
                {
                    user.ChangeAnimation("Fall");
                }
                else
                {
                    isExit = true;
                }
            }
        }

        public override void Exit()
        {
            
        }

        public override void OnTransition()
        {
            if (isExit)
            {
                user.ChangeState(PlayerState.Idle);
            }
        }
    }

}
