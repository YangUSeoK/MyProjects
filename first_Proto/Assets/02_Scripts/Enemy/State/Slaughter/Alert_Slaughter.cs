using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert_Slaughter : EnemyState
{
    public Alert_Slaughter(Enemy _enemy) : base("Alert", _enemy) { }

    public override void EnterState()
    {
        Debug.Log("Alert ÀÔÀå!");
    }

    public override void ExitState()
    {
        Debug.Log("Alert ÅðÀå!");
    }

    public override void Action()
    {
        Debug.Log("Alert ¹°¸®¾÷µ«!");
    }

    public override void CheckState()
    {
        Debug.Log("Alert ¾÷µ«!");
        float dist = Vector3.Distance(m_Enemy.PlayerTr.position, m_Enemy.transform.position);
        if (dist <= m_Enemy.PatrolPlayerDetectRange)
        {
            m_Enemy.SetState((m_Enemy as Enemy_Slaughter).TracePlayer);
        }
        else
        {
            // 5ÃÊ Áö³ª¸é 
            //m_Enemy.SetState(((Enemy_SightMan)m_Enemy).Patrol);
        }
    }

    private IEnumerator AggroCheckCoroutine()
    {
        yield return null;
    }
}
