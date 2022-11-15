using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle_Listener : EnemyState
{
    public Idle_Listener(Enemy _enemy) : base("Patrol", _enemy) { }
 
    public override void EnterState()
    {
        Debug.Log("Patrol ����!");
    }

    public override void ExitState()
    {
        Debug.Log("Patrol ����!");
    }

    public override void Action()
    {
        Debug.Log("Patrol ��������!");
    }

    public override void CheckState()
    {
        Debug.Log("Patrol ����!");
    }
}
