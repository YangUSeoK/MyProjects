using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class TracePlayer_Slaughter : EnemyState
{
    private Vector3 m_PlayerPos = Vector3.zero;
    public Vector3 PlayerPos
    {
        set { m_PlayerPos = value; }
    }
    bool mbIsLookPlayer = false;

    public TracePlayer_Slaughter(Enemy _enemy) : base("TracePlayer", _enemy) { }
 
    public override void EnterState()
    {
        Debug.Log("TracePlayer ����!");
        Debug.Log("�ֺ� ���� �θ��ϴ�!");
    }

    public override void ExitState()
    {
        Debug.Log("TracePlayer ����!");
    }

    public override void Action()
    {
        // �÷��̾�� ���̸� ���� ��ġ���� ������Ʈ ����
        RaycastHit hitInfo;
        if(Physics.Raycast(m_Enemy.transform.position, m_PlayerPos - m_Enemy.transform.position, out hitInfo, m_Enemy.TraceDetectRange, m_FOV.mLayerMask))
        {
            if (hitInfo.collider.CompareTag("PLAYER"))
            {
                mbIsLookPlayer = true;
            }
        }
        else
        {
            mbIsLookPlayer = false;
        }
        m_Agent.destination = hitInfo.transform.position;
    }

    public override void CheckState()
    {
        Debug.Log("TracePlayer ����!");
        float dist = Vector3.Distance(m_Enemy.PlayerTr.position, m_Enemy.transform.position);

        // ���ݻ�Ÿ� �����̶�� + ���� �����ִٸ�
        if (m_Enemy.AttackRange <= dist && mbIsLookPlayer)  
        {
            m_Enemy.SetState((m_Enemy as Enemy_Slaughter).Attack);
            return;
        }
        if (Vector3.Distance(m_PlayerPos, m_Enemy.transform.position) <= 0.5f)
        {
            m_Enemy.SetState((m_Enemy as Enemy_Slaughter).Alert);
        }
        
    }


}
