using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SoundMan : Enemy
{
    private Idle_SoundMan m_Idle;
    public Idle_SoundMan Idle
    {
        get { return m_Idle; }
    }
    private Alert_SoundMan m_Alert;
    public Alert_SoundMan Alert
    {
        get { return m_Alert; }
    }
    private Trace_SoundMan m_Tarce;
    public Trace_SoundMan Trace
    {
        get { return m_Tarce; }
    }
    private Attack m_Attack;
    public Attack Attack
    {
        get { return m_Attack; }
    }




    protected void Awake()
    {
        m_Idle = new Idle_SoundMan(this);
        m_Alert = new Alert_SoundMan(this);
        m_Tarce = new Trace_SoundMan(this);
        m_Attack = new Attack(this);
    }

    public override void SetAlert()
    {
    }

    public override void SetAttack()
    {
    }

    public override void SetPatrol()
    {
    }

    public override void SetTrace()
    {
    }
}
