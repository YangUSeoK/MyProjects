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
    }

    public override void ExitState()
    {
        Debug.Log("TracePlayer 퇴장!");
    }

    public override void Action()
    {
        // 플레이어에게 레이를 쏴서 위치정보 업데이트 받음
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
        Debug.Log("TracePlayer 업뎃!");
        float dist = Vector3.Distance(m_Enemy.PlayerTr.position, m_Enemy.transform.position);

        // 공격사거리 안쪽이라면 + 직접 보고있다면
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
