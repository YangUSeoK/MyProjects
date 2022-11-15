using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Enemy_Slaughter : Enemy
{
    // EnemyManager∞° ∏‘ø©¡‡æﬂ «‘
    [SerializeField] protected FlagManager m_FlagManager;

    // EnemyState, «¡∑Œ∆€∆º
    #region EnemyState
    private Patrol_Slaughter m_Patrol = null;
    public Patrol_Slaughter Patrol
    {
        get { return m_Patrol; }
    }
    private TraceLight_Slaughter m_TraceLight = null;
    public TraceLight_Slaughter TraceLight
    {
        get { return m_TraceLight; }
    }

    private TracePlayer_Slaughter m_TracePlayer = null;
    public TracePlayer_Slaughter TracePlayer
    {
        get { return m_TracePlayer; }
    }
    private Alert_Slaughter m_Alert = null;
    public Alert_Slaughter Alert
    {
        get { return m_Alert; }
    }
    private Attack m_Attack = null;
    public Attack Attack
    {
        get { return m_Attack; }
    }
    #endregion

    private Transform m_LightTr = null;
    public Transform LightTr
    {
        get { return m_LightTr; }
        set { m_LightTr = value; }
    }

    // FOV
    private FOV m_FOV = null;


   

    protected override void Awake()
    {
        base.Awake();
        m_Patrol = new Patrol_Slaughter(this);
        m_TraceLight = new TraceLight_Slaughter(this);
        m_TracePlayer = new TracePlayer_Slaughter(this);
        m_Alert = new Alert_Slaughter(this);
        m_Attack = new Attack(this);

        m_FOV = GetComponent<FOV>();
    }

    protected override EnemyState GetInitialState()
    {
        return m_Patrol;
    }

    public override void SetPatrol()
    {
        m_Patrol.FOV = m_FOV;
        m_Patrol.Agent = m_Agent;
        m_Patrol.MoveSpeed = m_PatrolSpeed;
        m_Patrol.Flags = m_FlagManager.Flags;
    }

    public void SetTraceLight()
    {

    }

    public override void SetTracePlayer()
    {
        m_Patrol.FOV = m_FOV;
        m_TracePlayer.MoveSpeed = m_TracePlayerSpeed;
    }

    public override void SetAlert()
    {
        m_Patrol.FOV = m_FOV;
        m_Alert.MoveSpeed = m_AlertSpeed;
    }

    public override void SetAttack()
    {
    }
}
