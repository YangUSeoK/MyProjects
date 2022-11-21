using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class TracePlayer_Slaughter : EnemyState
{
    public TracePlayer_Slaughter(Enemy _enemy) : base("TracePlayer", _enemy) { }
    
    private Vector3 m_PlayerPos = Vector3.zero;
    public Vector3 PlayerPos
    {
        set { m_PlayerPos = value; }
    }
    private bool mbIsLookPlayer = false;

    private float m_Timer = 0f;

    public override void EnterState()
    {
        Debug.Log("TracePlayer ����!");
        Debug.Log("�ֺ� ���� �θ��ϴ�!");
        (m_Enemy as Enemy_Slaughter).SetTracePlayer();
        m_Agent.destination = m_PlayerPos;
        m_Timer = 0f;
    }

    public override void ExitState()
    {
        Debug.Log("TracePlayer ����!");
    }

    public override void Action()
    {
        // �÷��̾�� ���̸� ���� ��ġ���� ������Ʈ ����
        RaycastHit hitInfo;       
        int layerMask = (m_FOV.PlayerLayer | m_FOV.ObstacleLayer);

        Debug.DrawLine(m_Enemy.transform.position, m_Enemy.PlayerTr.position, Color.blue);

        if (Physics.Raycast(m_Enemy.transform.position, m_Enemy.PlayerTr.position - m_Enemy.transform.position,
            out hitInfo, m_Enemy.TraceDetectRange+ 30f, layerMask))
        {
            Debug.Log(hitInfo.transform.name);
            if (hitInfo.collider.CompareTag("PLAYER"))
            {
                Debug.Log("�÷��̾� ����");
                mbIsLookPlayer = true;
                m_PlayerPos = hitInfo.transform.position;
            }
            else
            {
                mbIsLookPlayer = false;
            }
        }
        else
        {
            mbIsLookPlayer = false;
        }
        m_Agent.destination = m_PlayerPos;
    }

    public override void CheckState()
    {
        float dist = Vector3.Distance(m_Enemy.PlayerTr.position, m_Enemy.transform.position);

        if (mbIsLookPlayer)
        {
            m_Timer = 0;
            if (m_Enemy.AttackRange >= dist)
            {
                Debug.Log("�׾�� �װ�!");
                m_Enemy.SetState((m_Enemy as Enemy_Slaughter).Attack);
                return;
            }
        }
        else
        {
            m_Timer += Time.deltaTime;
            Debug.Log($"Trace Player : {m_Timer}");

            if (m_Timer >= 5f)
            {
                Debug.Log("�����?");
                m_Enemy.SetState((m_Enemy as Enemy_Slaughter).Alert);
            }
        }




    }


}
