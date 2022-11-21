using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Listener : Enemy
{
    private Idle_Listener m_Idle;
    public Idle_Listener Idle
    {
        get { return m_Idle; }
    }
    private Trace_Listener m_TarcePlayer;
    public Trace_Listener TracePlayer
    {
        get { return m_TarcePlayer; }
    }
    private Alert_Listener m_Alert;
    public Alert_Listener Alert
    {
        get { return m_Alert; }
    }
    private Attack m_Attack;
    public Attack Attack
    {
        get { return m_Attack; }
    }


    protected override void Awake()
    {
        base.Awake();
        m_Idle = new Idle_Listener(this);
        m_TarcePlayer = new Trace_Listener(this);
        m_Alert = new Alert_Listener(this);
        m_Attack = new Attack(this);
    }

    public override void SetPatrol()
    {

    }

    public override void SetTracePlayer()
    {
    }

    public override void SetAlert()
    {
    }

    public override void SetAttack()
    {
    }

    

    
}
