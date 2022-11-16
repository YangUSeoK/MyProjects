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
        Debug.Log("TracePlayer 입장!");
        Debug.Log("주변 좀비를 부릅니다!");
        (m_Enemy as Enemy_Slaughter).SetTracePlayer();
        m_Agent.destination = m_PlayerPos;
    }

    public override void ExitState()
    {
        Debug.Log("TracePlayer 퇴장!");
    }

    public override void Action()
    {
        // 플레이어에게 레이를 쏴서 위치정보 업데이트 받음
        RaycastHit hitInfo;       // 맞으면 안되는놈들                                맞아야 하는 놈들
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

        // 공격사거리 안쪽이라면 + 직접 보고있다면
        if (m_Enemy.AttackRange >= dist && mbIsLookPlayer)
        {
            Debug.Log("죽어라 닝겐!");
            m_Enemy.SetState((m_Enemy as Enemy_Slaughter).Attack);
            return;
        }

        if (Vector3.Distance(m_PlayerPos, m_Enemy.transform.position) <= 0.5f)
        {
            Debug.Log("어디갔지?");
            m_Enemy.SetState((m_Enemy as Enemy_Slaughter).Alert);
        }

    }


}
