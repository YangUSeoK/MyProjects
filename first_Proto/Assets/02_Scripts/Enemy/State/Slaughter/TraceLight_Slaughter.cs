using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceLight_Slaughter : EnemyState
{
    public TraceLight_Slaughter(Enemy _enemy) : base("TraceLight", _enemy) { }

    private Vector3 m_LightPos = Vector3.zero;
    public Vector3 LightPos
    {
        set { m_LightPos = value; }
    }
    private Transform m_FlashTr = null;
    public Transform FlashTr
    {
        set { m_FlashTr = value; }
    }



    public override void EnterState()
    {
        Debug.Log("TraceLight ����!");
        (m_Enemy as Enemy_Slaughter).SetTraceLight();   // TraceLight �� Enemy_Slaughter���� �־ ����ȯ.
        m_Agent.destination = m_LightPos;
    }

    public override void Action()
    {
        // ���� ���̴��� üũ -> ���� ���̸� �� ��ġ�� ����
        // ������ ��ġ�� �̵�
        m_FOV.IsInFovWithRayCheckDirect(m_Enemy.TraceDetectRange, m_Enemy.TraceDetectAngle, "LIGHT", ref m_LightPos, ref m_FlashTr);
        m_Agent.destination = m_LightPos;
    }

    public override void CheckState()
    {
        // ������ ��ġ�� ��� ���̸� ���.
        RaycastHit hitInfo;

        // ���θ��°� ���� �Ÿ��� Ž������ ���̶��
        if (Physics.Raycast(m_Enemy.transform.position, m_FlashTr.position - m_Enemy.transform.position, out hitInfo, m_Enemy.TraceDetectRange, (1 << LayerMask.NameToLayer("FLASH"))))
        {
            // �÷��̾ �������� ����ִٸ� => TracePlayer
            // 20221116 ��켮:  �÷��̾�� �Ÿ� ������ ���纸�� �����ؾ� ��.
            if (Vector3.Distance(m_Enemy.PlayerTr.position, m_FlashTr.position) <= 1.5f)
            {
                m_Enemy.SetState((m_Enemy as Enemy_Slaughter).TracePlayer);
                return;
            }

            // else
        }

        // ���̰� �ȸ¾Ұ� ������ �� ��ġ�� ���� ��ġ�� ������  => Alert
        if (Vector3.Distance(m_LightPos, m_Enemy.transform.position) <= 0.5f)
        {
            m_Enemy.SetState((m_Enemy as Enemy_Slaughter).Alert);
        }
    }



    public override void ExitState()
    {
        Debug.Log("TraceLight ����!");
    }
}
