using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trace_Listener : EnemyState
{
    public Trace_Listener(Enemy _enemy) : base("Trace", _enemy) { }
   
    public override void EnterState()
    {
        Debug.Log("Trace ����!");
    }

    public override void ExitState()
    {
        Debug.Log("Trace ����!");
    }

    public override void Action()
    {
        Debug.Log("Trace ��������!");
    }

    public override void CheckState()
    {
        Debug.Log("Trace ����!");
        //if(���ݻ�Ÿ� �ȿ� �ִٸ� && ������ �տ� �ִٸ�)
        m_Enemy.SetState((m_Enemy as Enemy_Slaughter).Attack);
    }
}
