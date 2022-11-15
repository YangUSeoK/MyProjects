using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trace_Slaughter : EnemyState
{
    public Trace_Slaughter(Enemy _enemy) : base("Trace", _enemy) { }
 
    public override void EnterState()
    {
        Debug.Log("Trace 입장!");
        m_FOVForPlayer.SetFOV(m_Enemy.PlayerTr, m_Enemy.TraceDetectRange, m_Enemy.TraceDetectAngle);
    }

    public override void ExitState()
    {
        Debug.Log("Trace 퇴장!");
    }

    public override void Action()
    {
        Debug.Log("Trace 물리업뎃!");
    }

    public override void CheckState()
    {
        Debug.Log("Trace 업뎃!");
        float dist = Vector3.Distance(m_Enemy.PlayerTr.position, m_Enemy.transform.position);
        if (m_Enemy.AttackRange <= dist)  // 공격사거리 안쪽이라면 + 부채꼴 범위안에 있다면
        {  
            m_Enemy.SetState((m_Enemy as Enemy_Slaughter).Attack);
        }
        else
        {
            // 5초 지나면
            //m_Enemy.SetState(((Enemy_SightMan)m_Enemy).Alert);
        }
    }


}
