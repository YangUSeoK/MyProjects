using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : EnemyState
{
    public Attack(Enemy _enemy) : base("Attack", _enemy) { }

    public override void EnterState()
    {
        Debug.Log("Attack ����!");
    }

    public override void ExitState()
    {
        Debug.Log("Attack ����!");
    }

    public override void Action()
    {
        Debug.Log("Attack ��������!");
    }

    public override void CheckState()
    {
        Debug.Log("Attack ����!");
    }
}
