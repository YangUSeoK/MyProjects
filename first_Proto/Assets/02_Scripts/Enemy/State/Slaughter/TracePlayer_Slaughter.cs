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
        (m_Enemy as Enemy_Slaughter).SetTracePlayer();
        m_Agent.destination = m_PlayerPos;
    }

    public override void ExitState()
    {
        Debug.Log("TracePlayer ����!");
    }

    public override void Action()
    {
        // �÷��̾�� ���̸� ���� ��ġ���� ������Ʈ ����
        RaycastHit hitInfo;       // ������ �ȵǴ³��                                �¾ƾ� �ϴ� ���
        int layerMask = (~(m_FOV.LightLayer | m_FOV.FlashLayer)   |   (m_FOV.PlayerLayer | m_FOV.ObstacleLayer));

        if (Physics.Raycast(m_Enemy.transform.position, m_Enemy.PlayerTr.position - m_Enemy.transform.position, out hitInfo, m_Enemy.TraceDetectRange, layerMask))
        {
            if (hitInfo.collider.CompareTag("PLAYER"))
            {
                mbIsLookPlayer = true;
                m_PlayerPos = hitInfo.transform.position;
            }
        }
        else
        {
            mbIsLookPlayer = false;
        }

        if (mbIsLookPlayer)
        {
            m_Agent.destination = hitInfo.transform.position;
        }
    }

    public override void CheckState()
    {
        float dist = Vector3.Distance(m_Enemy.PlayerTr.position, m_Enemy.transform.position);

        // ���ݻ�Ÿ� �����̶�� + ���� �����ִٸ�
        if (m_Enemy.AttackRange >= dist && mbIsLookPlayer)
        {
            Debug.Log("�׾�� �װ�!");
            m_Enemy.SetState((m_Enemy as Enemy_Slaughter).Attack);
            return;
        }

        if (Vector3.Distance(m_PlayerPos, m_Enemy.transform.position) <= 0.5f)
        {
            Debug.Log("�����?");
            m_Enemy.SetState((m_Enemy as Enemy_Slaughter).Alert);
        }

    }


}
