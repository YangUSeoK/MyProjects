using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert_Listener : EnemyState
{
    public Alert_Listener(Enemy _enemy) : base("Alert", _enemy) { }
 
    public override void EnterState()
    {
        Debug.Log("Alert ����!");
    }

    public override void ExitState()
    {
        Debug.Log("Alert ����!");
    }

    public override void Action()
    {
        Debug.Log("Alert ��������!");
    }

    public override void CheckState()
    {
        Debug.Log("Alert ����!");
    }
}
