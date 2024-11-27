using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBossHitPoint : MonoBehaviour
{
    public ForestBoss forestBoss;
    // Start is called before the first frame update
    void Start()
    {
        forestBoss = GetComponent<ForestBoss>();
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("PlayerTreeAttackObjects"))
        {
            forestBoss.ChangeState(Forest_Boss_States.ForestBossState.Hit);
        }
    }
}
