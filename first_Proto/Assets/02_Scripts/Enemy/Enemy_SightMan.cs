using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Enemy_SightMan : Enemy
{
    // EnemyManager∞° ∏‘ø©¡‡æﬂ «‘
    [SerializeField] protected FlagManager m_FlagManager;

    // EnemyState, «¡∑Œ∆€∆º
    private Patrol_SightMan m_Patrol = null;
    public Patrol_SightMan Patrol
    {
        get { return m_Patrol; }
    }
    private Alert_SightMan m_Alert = null;
    public Alert_SightMan Alert
    {
        get { return m_Alert; }
    }
    private Trace_SightMan m_Trace = null;
    public Trace_SightMan Trace
    {
        get { return m_Trace; }
    }
    private Attack m_Attack = null;
    public Attack Attack
    {
        get { return m_Attack; }
    }

    private Transform m_LightTr = null;
    public Transform LightTr
    {
        get { return m_LightTr; }
        set { m_LightTr = value; }
    }

    // FOV
    private FOVForPlayer m_FOVForPlayer = null;
    private FOVForLight m_FOVForLight = null;


    protected  void Awake()
    {
        m_Patrol = new Patrol_SightMan(this);
        m_Alert = new Alert_SightMan(this);
        m_Trace = new Trace_SightMan(this);
        m_Attack = new Attack(this);

        m_FOVForPlayer = GetComponent<FOVForPlayer>();
        m_FOVForLight = GetComponent<FOVForLight>();
    }

    protected override EnemyState GetInitialState()
    {
        return m_Patrol;
    }

    public override void SetPatrol()
    {
        m_Patrol.FOVForPlayer = m_FOVForPlayer;
        m_Patrol.FOVForLight = m_FOVForLight;
        m_Patrol.MoveSpeed = m_PatrolSpeed;
        m_Patrol.Flags = m_FlagManager.Flags;
    }

    public override void SetAlert()
    {
        m_Patrol.FOVForPlayer = m_FOVForPlayer;
        m_Alert.MoveSpeed = m_AlertSpeed;
    }

    public override void SetTrace()
    {
        m_Patrol.FOVForPlayer = m_FOVForPlayer;
        m_Trace.MoveSpeed = m_TraceSpeed;
    }

    public override void SetAttack()
    {
    }
}
