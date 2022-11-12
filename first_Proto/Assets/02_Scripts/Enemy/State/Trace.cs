using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trace : EnemyState
{
    public Trace(EnemyAI _enemyAI)
    {
        m_EnemyAI = _enemyAI;
    }

    public override void EnterState(EnemyAI _enemyAI)
    {
        Debug.Log("Trace ÀÔÀå!");
    }

    public override void ExitState(EnemyAI _enemyAI)
    {
        Debug.Log("Trace ÅðÀå!");
    }

    public override void FixedUpdateLogic(EnemyAI _enemyAI)
    {
        Debug.Log("Trace ¹°¸®¾÷µ«!");
    }

    public override void UpdateLogic(EnemyAI _enemyAI)
    {
        Debug.Log("Trace ¾÷µ«!");
    }
}
