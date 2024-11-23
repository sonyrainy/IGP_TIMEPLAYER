using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Player_States
{
    [System.Serializable]
    public enum PlayerState
    {
        Spawn, Idle, Run, Jump, Dash, Hit, Last
    }

    public enum PlayerAnimation
    {
        Player_Spawn,
        Player_Idle,
        Player_Run,
        Player_Jump,
        Player_Dash,
        Player_Hit,
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
            Debug.Log("Player: Idle State");
            user.ChangeAnimation(PlayerAnimation.Player_Idle);
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
            Debug.Log("Player: Run State");
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
            Debug.Log("Player: Jump State");
            hasJumped = false;
        }

        public override void Execute()
        {
            if (!hasJumped && user.isGround)
            {
                user.yVelocity = user.jumpForce;
                user.isGround = false;
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
            }
        }
    }

    public class Dash : State<Player>
    {
        private bool isAnimationComplete = false;
        private bool isExit = false;
        private Vector2 preVelVec;

        public Dash(Player user) : base(user) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Player: Dash State");
            Vector2 preVelVec = new Vector2(user.rigidbody.velocity.x, 0);
            user.ChangeAnimation(PlayerAnimation.Player_Dash);
            isAnimationComplete = false;
            isExit = false;
        }

        public override void Execute()
        {
            if (!isAnimationComplete)
            {
                user.isDash = true;
                user.yVelocity = 0;
                Vector2 dashVec = new Vector2(Input.GetAxisRaw("Horizontal") * user.dashFloat, user.yVelocity);
                user.rigidbody.velocity = dashVec;

                AnimatorStateInfo stateInfo = user.animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("Player_Dash") && stateInfo.normalizedTime >= 0.7f)
                {
                    isAnimationComplete = true;
                }
            }
            else
            {
                user.isDash = false;
                user.rigidbody.velocity = preVelVec;

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
            if (isExit && user.isGround)
            {
                if (Input.GetAxisRaw("Horizontal") != 0)
                {
                    user.ChangeState(PlayerState.Run);
                } 
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    user.ChangeState(PlayerState.Jump);
                }
                else
                {
                    user.ChangeState(PlayerState.Idle);
                }
                
            }
        }
    }

    public class Hit : State<Player>
    {
        private bool isAnimationComplete = false;
        private bool isExit = false;

        public Hit(Player user) : base(user) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Player: Hit State");
            user.ChangeAnimation(PlayerAnimation.Player_Hit);
            isAnimationComplete = false;
            isExit = false;
        }

        public override void Execute()
        {
            if (!isAnimationComplete)
            {
                user.rigidbody.velocity = new Vector2(0, user.rigidbody.velocity.y);

                AnimatorStateInfo stateInfo = user.animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("Player_Hit") && stateInfo.normalizedTime >= 1f)
                {
                    isAnimationComplete = true;
                }
            }
            else
            {
                if (user.isGround == false)
                {
                    user.ApplyGravity();
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
