using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Player_States
{
    [System.Serializable]
    public enum PlayerState
    {
        Spawn, Idle, Run, Jump, Dash, Hit, Fall, Last
    }

    public enum PlayerAnimation
    {
        Player_Spawn,
        Player_Idle,
        Player_Run,
        Player_Jump,
        Player_Dash,
        Player_Hit,
        Player_Fall,
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
            user.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
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

            user.GetComponent<Rigidbody2D>().velocity = new Vector2(user.moveSpeed * Input.GetAxisRaw("Horizontal"), user.GetComponent<Rigidbody2D>().velocity.y);

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

            if (!user.isGround)
            {
                user.ChangeState(PlayerState.Fall);
            }
        }
    }

    public class Jump : State<Player>
    {
        private bool hasJumped = false; // 무한 점프를 방지하기 위한 bool

        public Jump(Player user) : base(user) { }
        
        public override void Enter()
        {
            base.Enter();
            Debug.Log("Player: Jump State");
            hasJumped = false;
        }

        public override void Execute()
        {
            // 점프 후 공중에서 좌 우 이동 가능
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                Vector3 scale = user.transform.localScale;
                scale.x = Input.GetAxisRaw("Horizontal");
                user.transform.localScale = scale;
                
                user.GetComponent<Rigidbody2D>().velocity = new Vector2(user.moveSpeed * Input.GetAxisRaw("Horizontal"), user.GetComponent<Rigidbody2D>().velocity.y);
            }

            // 점프 실행 코드
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

                hasJumped = !hasJumped;
            }

            if (user.isGround == true)
            {
                if (Input.GetAxisRaw("Horizontal") == 0)
                {
                    user.ChangeState(PlayerState.Idle);
                    hasJumped = !hasJumped;
                }
                else
                {
                    user.ChangeState(PlayerState.Run);
                    hasJumped = !hasJumped;
                }
                
                hasJumped = !hasJumped;
            }
        }
    }

    public class Dash : State<Player>
    {
        private bool isAnimationComplete = false; // 애니메이션이 끝까지 진행 되었는지 확인하기 위한 bool 변수
        private bool isExit = false; // Dash 상태를 나가도 되는 지 확인하기 위한 bool변수
        private Vector2 preVelVec; // Dash 진입 직전의 player.Rigidbody2D.velocity를 저장하기 위한 Vector2 변수

        public Dash(Player user) : base(user) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Player: Dash State");
            user.ChangeAnimation(PlayerAnimation.Player_Dash);

            // 대쉬 상태를 진입할 때마다 모든 지역 bool 변수를 false로 초기화
            isAnimationComplete = false;
            isExit = false;
        }

        public override void Execute()
        {
            if (!isAnimationComplete)
            {
                // 애니메이션 방향 조정
                if (Input.GetAxisRaw("Horizontal") != 0)
                {
                    Vector3 scale = user.transform.localScale;
                    scale.x = Input.GetAxisRaw("Horizontal");
                    user.transform.localScale = scale;
                }

                // 대쉬 시작 및 중력 제거
                user.isDash = true;
                user.yVelocity = 0;

                // 애니메이션 방향에 맞게, 그리고 dashFloat값에 따라 대쉬
                Vector2 dashVec = new Vector2(Input.GetAxisRaw("Horizontal") * user.dashFloat, user.yVelocity);
                user.GetComponent<Rigidbody2D>().velocity = dashVec;

                // 애니메이션이 70%까지 진행되면 애니메이션 바로 종료
                AnimatorStateInfo stateInfo = user.animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("Player_Dash") && stateInfo.normalizedTime >= 0.7f)
                {
                    isAnimationComplete = true;
                }
            }
            else
            {
                // 대쉬 상태 종료 및 이전에 저장한 player.Rigidbody2D.velocity를 다시 가져옴
                user.isDash = false;
                user.GetComponent<Rigidbody2D>().velocity = preVelVec;

                if (user.isGround == false)
                {
                    // 대쉬 종료 후 플레이어가 바닥에 붙어있지 않으면 낙하 애니메이션 진행
                    user.ChangeAnimation("Fall");
                }
                else
                {
                    // 바닥에 붙어 있으면 상태 종료
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
            user.isDash = false;
            user.ChangeAnimation(PlayerAnimation.Player_Hit);
            isAnimationComplete = false;
            isExit = false;
        }

        public override void Execute()
        {
            if (!isAnimationComplete)
            {
                // 플레이어 피격 시 모든 속도를 0으로 만듬. (중력은 계속 적용, Player.cs의 Update함수 참고)
                user.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

                // 애니메이션이 100%가 되면 상태를 종료하기 위한 지연 코드
                AnimatorStateInfo stateInfo = user.animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("Player_Hit") && stateInfo.normalizedTime >= 1f)
                {
                    isAnimationComplete = true;
                }
            }
            else
            {
                isExit = true;
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
                else
                {
                    user.ChangeState(PlayerState.Idle);
                }
            }
        }
    }

    public class Fall : State<Player>
    {
        public Fall(Player user) : base(user) { }
        public override void Enter()
        {
            base.Enter();
            Debug.Log("Player: Fall State");
            user.ChangeAnimation("Fall");
        }
        public override void Execute()
        {

        }
        public override void Exit()
        {

        }
        public override void OnTransition()
        {
            if (user.isGround)
            {
                user.ChangeState(PlayerState.Idle);
            }
        }
    }
}
