using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert : EnemyState
{
    public Alert(EnemyAI _enemyAI)
    {
        m_EnemyAI = _enemyAI;
    }

    public override void EnterState(EnemyAI _enemyAI)
    {
        Debug.Log("Alert ÀÔÀå!");
    }

    public override void ExitState(EnemyAI _enemyAI)
    {
        Debug.Log("Alert ÅðÀå!");
    }

    public override void FixedUpdateLogic(EnemyAI _enemyAI)
    {
        Debug.Log("Alert ¹°¸®¾÷µ«!");
    }

    public override void UpdateLogic(EnemyAI _enemyAI)
    {
        Debug.Log("Alert ¾÷µ«!");
    }
}
