using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T> where T : MonoBehaviour
{
    public T user;

    public State(T user)
    {
        this.user = user;
    }
    public virtual void Enter()
    {
        //Debug.Log($"Enter {this.ToString()}");
    }
    public abstract void Execute();
    public abstract void Exit();
    public abstract void OnTransition();
}

