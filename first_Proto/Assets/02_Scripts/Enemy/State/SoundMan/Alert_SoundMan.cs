using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert_SoundMan : EnemyState
{
    public Alert_SoundMan(Enemy _enemy) : base("Alert", _enemy) { }
 
    public override void EnterState()
    {
        Debug.Log("Alert ÀÔÀå!");
    }

    public override void ExitState()
    {
        Debug.Log("Alert ÅðÀå!");
    }

    public override void Action()
    {
        Debug.Log("Alert ¹°¸®¾÷µ«!");
    }

    public override void CheckState()
    {
        Debug.Log("Alert ¾÷µ«!");
    }
}
