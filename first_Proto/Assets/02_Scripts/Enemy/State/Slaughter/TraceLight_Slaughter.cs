using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceLight_Slaughter : EnemyState
{
    public TraceLight_Slaughter(Enemy _enemy) : base("TraceLight", _enemy) { }

    private Vector3 m_LightPos;
    private Vector3 m_FlashPos;


public override void EnterState()
    {
        Debug.Log("TraceLight 입장!");
    }

    public override void Action()
    {
        // 빛이 보이는지 체크 -> 빛이 보이면 빛 위치를 저장
        // 저장한 위치로 이동
        Debug.Log("TraceLight 액션!");

    }

    public override void CheckState()
    {
        Debug.Log("TraceLight 체크체크!");
        if (Vector3.Distance(m_LightPos, m_Enemy.transform.position)  <= 0.5f)
        {

        }

        // 손전등 위치로 계속 레이를 쏜다.
        
        // if(가로막는게 없고 거리가 일정길이 안이라면)


        // 마지막 빛 위치랑 현재 위치가 같고(입실론 계산) 레이가 안맞으면

    }

    

    public override void ExitState()
    {
        Debug.Log("TraceLight 퇴장!");
    }
}
