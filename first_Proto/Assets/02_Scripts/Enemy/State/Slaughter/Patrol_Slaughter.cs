using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class Patrol_Slaughter : EnemyState
{
    // ������
    public Patrol_Slaughter(Enemy _enemy) : base("Patrol", _enemy) { }

    // Patrol �ʱ⼼��
    private Flag[] m_Flags;
    public Flag[] Flags
    {
        set { m_Flags = value; }
    }

    private int mNextIdx = 0;

    public override void EnterState()
    {
        Debug.Log("Patrol ����!");

        // ������Ʈ�� ������ �������ش�.
        if (m_Flags == null)
        {
            m_Enemy.SetPatrol();
        }
    }

    public override void ExitState()
    {
        Debug.Log("Patrol ����!");
    }

    public override void Action()
    {
        Debug.Log("Patrol �׼�!");
        // ���� �� �� ���
        if (m_Agent.remainingDistance <= 0.5f)
        {
            // �ε����� ����ȣ���� ���� �ٽ� ó������. %�� ������
            ++mNextIdx;
            if (mNextIdx >= m_Flags.Length)
            {
                mNextIdx -= m_Flags.Length;
            }
            PatrollFlags();
        }
    }

    public override void CheckState()
    {
        Debug.Log("Patrol CheckState!");
        float dist = Vector3.Distance(m_Enemy.PlayerTr.position, m_Enemy.transform.position);

        // �÷��̾ �����ȿ� ��������
        if (m_FOV.IsInFOV(m_Enemy.PatrolPlayerDetectRange, m_Enemy.PatrolDetectAngle, LayerMask.NameToLayer("PLAYER"))
            && m_FOV.IsLookDirect(m_Enemy.PlayerTr, m_Enemy.PatrolPlayerDetectRange, LayerMask.NameToLayer("PLAYER")))
        {
            m_Enemy.SetState((m_Enemy as Enemy_Slaughter).TracePlayer);
            return;
        }

        // ���� �����ȿ� ��������
        // ������ �ι� �˻��ؾ��ϴ°Ŵϱ� else if �Ƚᵵ ����.
        Vector3 lightPos = Vector3.zero;
        if (m_FOV.IsInDirectFovWithRay(m_Enemy.PatrolDetectRange, m_Enemy.PatrolDetectAngle, LayerMask.NameToLayer("LIGHT"), ref lightPos)) 
        {
            m_Enemy.SetState((m_Enemy as Enemy_Slaughter).TraceLight);
            return;
        }
    }

    public void PatrollFlags()
    {
        // ��� ������� ���� ����
        if (m_Agent.isPathStale)
        {
            return;
        }
        m_Agent.destination = m_Flags[mNextIdx].transform.position;
        m_Agent.isStopped = false;
    }
}
