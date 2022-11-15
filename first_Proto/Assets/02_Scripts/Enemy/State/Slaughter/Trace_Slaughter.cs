using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trace_Slaughter : EnemyState
{
    public Trace_Slaughter(Enemy _enemy) : base("Trace", _enemy) { }
 
    public override void EnterState()
    {
        Debug.Log("Trace ����!");
        m_FOVForPlayer.SetFOV(m_Enemy.PlayerTr, m_Enemy.TraceDetectRange, m_Enemy.TraceDetectAngle);
    }

    public override void ExitState()
    {
        Debug.Log("Trace ����!");
    }

    public override void Action()
    {
        Debug.Log("Trace ��������!");
    }

    public override void CheckState()
    {
        Debug.Log("Trace ����!");
        float dist = Vector3.Distance(m_Enemy.PlayerTr.position, m_Enemy.transform.position);
        if (m_Enemy.AttackRange <= dist)  // ���ݻ�Ÿ� �����̶�� + ��ä�� �����ȿ� �ִٸ�
        {  
            m_Enemy.SetState((m_Enemy as Enemy_Slaughter).Attack);
        }
        else
        {
            // 5�� ������
            //m_Enemy.SetState(((Enemy_SightMan)m_Enemy).Alert);
        }
    }


}
