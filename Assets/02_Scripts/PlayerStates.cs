using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player_States
{
    [System.Serializable]
    public enum PlayerState
    {
        Idle, Run, Dash
    }

    public enum PlayerAnimation
    {
        Player_Idle,
        Player_Run,
        Player_Dash
    }

    public class Idle : State<Player>
    {
        public Idle(Player user) : base(user) { }

        public override void Enter()
        {
            base.Enter();
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
        }

        public override void Execute()
        {
            
        }

        public override void Exit()
        {
            throw new System.NotImplementedException();
        }

        public override void OnTransition()
        {
            throw new System.NotImplementedException();
        }
    }
}


public class PlayerStates : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
