using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracePlayer_Slaughter : EnemyState
{
    public TracePlayer_Slaughter(Enemy _enemy) : base("TracePlayer", _enemy) { }
 
    public override void EnterState()
    {
        Debug.Log("TracePlayer ����!");
        m_FOVForPlayer.SetFOV(m_Enemy.PlayerTr, m_Enemy.TraceDetectRange, m_Enemy.TraceDetectAngle);
    }

    public override void ExitState()
    {
        Debug.Log("TracePlayer ����!");
    }

    public override void Action()
    {
        Debug.Log("TracePlayer �׼�!");
    }

    public override void CheckState()
    {
        Debug.Log("TracePlayer ����!");
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
