using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        m_FOV.IsInFovWithRayCheckDirect(m_Enemy.TraceDetectRange, m_Enemy.TraceDetectAngle,
                                        "LIGHT", m_FOV.mLayerMask, ref m_LightPos, ref m_FlashTr);
        m_Agent.destination = new Vector3(m_LightPos.x, m_Enemy.transform.position.y, m_LightPos.z);
    }

    public override void CheckState()
    {
        // ������ ��ġ�� ��� ���̸� ���.
        RaycastHit hitInfo;
        int layerMask = (1 << m_FOV.FlashLayer) | (1 << m_FOV.ObstacleLayer) | (1 << m_FOV.PlayerLayer) | ~(1 << m_FOV.LightLayer);

        // Ž������ ���̶��
        if (Physics.Raycast(m_Enemy.transform.position, m_FlashTr.position - m_Enemy.transform.position,
            out hitInfo, m_Enemy.TraceDetectRange + 30f, layerMask))

            Debug.Log(hitInfo.transform.name);
        {
            // ���θ��°� ���� �÷��̾ �������� ����ִٸ� => TracePlayer
            // 20221116 ��켮:  �÷��̾�� �Ÿ� ������ ���纸�� �����ؾ� ��.
            if (hitInfo.collider.CompareTag("PLAYER") ||
                (hitInfo.collider.CompareTag("FLASH") && Vector3.Distance(m_Enemy.PlayerTr.position, m_FlashTr.position) <= 1.5f))
            {
                Debug.Log("���θ��°� ��� �Ų���!!");
                m_Enemy.SetState((m_Enemy as Enemy_Slaughter).TracePlayer);
                return;
            }

            //else
            //{
            //    Debug.Log("�ƹ��͵� ����?");
            //    m_Enemy.SetState((m_Enemy as Enemy_Slaughter).Alert);

            //    return;
            //}
        }

        Debug.DrawLine(m_Enemy.transform.position, m_LightPos, Color.blue);
        // ���̰� �ȸ¾Ұ� ������ �� ��ġ�� ���� ��ġ�� ������  => Alert
        if (Vector3.Distance(new Vector3(m_LightPos.x, m_Enemy.transform.position.y, m_LightPos.z), m_Enemy.transform.position) <= 0.3f)
        {
            Debug.Log("�ƹ��͵� ����?");
            m_Enemy.SetState((m_Enemy as Enemy_Slaughter).Alert);
        }
    }



    public override void ExitState()
    {
        Debug.Log("TraceLight ����!");
    }
}
