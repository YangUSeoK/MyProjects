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
        Debug.Log("TraceLight 입장!");
        (m_Enemy as Enemy_Slaughter).SetTraceLight();   // TraceLight 는 Enemy_Slaughter에만 있어서 형변환.
        m_Agent.destination = m_LightPos;
    }

    public override void Action()
    {
        // 빛이 보이는지 체크 -> 빛이 보이면 빛 위치를 저장
        // 저장한 위치로 이동
        m_FOV.IsInFovWithRayCheckDirect(m_Enemy.TraceDetectRange, m_Enemy.TraceDetectAngle, "LIGHT", ref m_LightPos, ref m_FlashTr);
        m_Agent.destination = m_LightPos;
    }

    public override void CheckState()
    {
        // 손전등 위치로 계속 레이를 쏜다.
        RaycastHit hitInfo;

        // 가로막는게 없고 거리가 탐지범위 안이라면
        if (Physics.Raycast(m_Enemy.transform.position, m_FlashTr.position - m_Enemy.transform.position, out hitInfo, m_Enemy.TraceDetectRange, (1 << LayerMask.NameToLayer("FLASH"))))
        {
            // 플레이어가 손전등을 들고있다면 => TracePlayer
            // 20221116 양우석:  플레이어랑 거리 실제로 맞춰보고 수정해야 함.
            if (Vector3.Distance(m_Enemy.PlayerTr.position, m_FlashTr.position) <= 1.5f)
            {
                m_Enemy.SetState((m_Enemy as Enemy_Slaughter).TracePlayer);
                return;
            }

            // else
        }

        // 레이가 안맞았고 마지막 빛 위치랑 현재 위치가 같으면  => Alert
        if (Vector3.Distance(m_LightPos, m_Enemy.transform.position) <= 0.5f)
        {
            m_Enemy.SetState((m_Enemy as Enemy_Slaughter).Alert);
        }
    }



    public override void ExitState()
    {
        Debug.Log("TraceLight 퇴장!");
    }
}
