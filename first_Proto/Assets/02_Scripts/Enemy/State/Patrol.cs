using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : EnemyState
{
    public Patrol(EnemyAI _enemyAI)
    {
        m_EnemyAI = _enemyAI;
    }

    public override void EnterState(EnemyAI _enemyAI)
    {
        Debug.Log("Patrol ÀÔÀå!");
    }

    public override void ExitState(EnemyAI _enemyAI)
    {
        Debug.Log("Patrol ÅðÀå!");
    }

    public override void FixedUpdateLogic(EnemyAI _enemyAI)
    {
        Debug.Log("Patrol ¹°¸®¾÷µ«!");
    }

    public override void UpdateLogic(EnemyAI _enemyAI)
    {
        Debug.Log("Patrol ¾÷µ«!");

        float timer = 0;
        while(true)
        {
            timer += Time.deltaTime;
            if (timer > 5f)
            {
                //_enemyAI.SetState();
            }
        }

    }
}
