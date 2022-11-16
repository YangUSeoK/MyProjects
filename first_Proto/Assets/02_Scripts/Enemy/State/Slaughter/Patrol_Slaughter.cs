using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class Patrol_Slaughter : EnemyState
{
    // 생성자
    public Patrol_Slaughter(Enemy _enemy) : base("Patrol", _enemy) { }

    // Patrol 초기세팅
    private Flag[] m_Flags;
    public Flag[] Flags
    {
        set { m_Flags = value; }
    }
    private int mNextIdx = 0;

    private Vector3 m_LightPos = Vector3.zero;
    private Transform m_FlashTr = null;


    public override void EnterState()
    {
        Debug.Log("Patrol 입장!");

        // 정찰루트가 없으면 설정해준다.
        if (m_Flags == null)
        {
            m_Enemy.SetPatrol();
        }
    }

    public override void ExitState()
    {
        Debug.Log("Patrol 퇴장!");
    }

    public override void Action()
    {
        // 다음 갈 곳 계산
        if (m_Agent.remainingDistance <= 0.5f)
        {
            // 인덱스가 끝번호까지 가면 다시 처음으로. %랑 같은거
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
        float dist = Vector3.Distance(m_Enemy.PlayerTr.position, m_Enemy.transform.position);

        // 플레이어가 범위안에 들어왔으면
        if (m_FOV.IsInFOV(m_Enemy.PatrolPlayerDetectRange, m_Enemy.PatrolDetectAngle, LayerMask.NameToLayer("PLAYER"))
            && m_FOV.IsLookDirect(m_Enemy.PlayerTr, m_Enemy.PatrolPlayerDetectRange, LayerMask.NameToLayer("PLAYER")))
        {
            Debug.Log("인간이다!!");
            m_Enemy.SetState((m_Enemy as Enemy_Slaughter).TracePlayer);
            return;
        }

        // 빛이 범위안에 들어왔으면
        // 어차피 두번 검사해야하는거니까 else if 안써도 같다.
        if (m_FOV.IsInFovWithRayCheckDirect(m_Enemy.PatrolDetectRange, m_Enemy.PatrolDetectAngle,
            "LIGHT", m_FOV.mLayerMask, ref m_LightPos, ref m_FlashTr)) 
        {
            Debug.Log("빛이다!! 누가 있나?");
            (m_Enemy as Enemy_Slaughter).SetToTraceLight(m_FlashTr, m_LightPos);
            (m_Enemy as Enemy_Slaughter).SetState((m_Enemy as Enemy_Slaughter).TraceLight);
            return;
        }
    }

    public void PatrollFlags()
    {
        // 경로 계산중일 때는 리턴
        if (m_Agent.isPathStale)
        {
            return;
        }
        m_Agent.destination = m_Flags[mNextIdx].transform.position;
        m_Agent.isStopped = false;
    }
}
