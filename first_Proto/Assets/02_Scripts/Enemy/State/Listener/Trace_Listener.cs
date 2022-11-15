using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trace_Listener : EnemyState
{
    public Trace_Listener(Enemy _enemy) : base("Trace", _enemy) { }
   
    public override void EnterState()
    {
        Debug.Log("Trace 입장!");
    }

    public override void ExitState()
    {
        Debug.Log("Trace 퇴장!");
    }

    public override void Action()
    {
        Debug.Log("Trace 물리업뎃!");
    }

    public override void CheckState()
    {
        Debug.Log("Trace 업뎃!");
        //if(공격사거리 안에 있다면 && 방향이 앞에 있다면)
        m_Enemy.SetState((m_Enemy as Enemy_Slaughter).Attack);
    }
}
