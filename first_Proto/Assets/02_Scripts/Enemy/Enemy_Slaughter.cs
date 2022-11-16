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

    private Transform m_FlashTr = null;
    public Transform FlashTr
    {
        get { return m_FlashTr; }
        set { m_FlashTr = value; }
    }

    // FOV
    private FOV m_FOV = null;
    private Vector3 m_LightPos = Vector3.zero;

   

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
        m_TraceLight.FOV = m_FOV;
        m_TraceLight.Agent = m_Agent;
        m_TraceLight.MoveSpeed = m_PatrolSpeed;
        m_TraceLight.LightPos = m_LightPos;
        m_TraceLight.FlashTr = m_FlashTr;
    }

    public override void SetTracePlayer()
    {
        m_TracePlayer.FOV = m_FOV;
        m_TracePlayer.Agent = m_Agent;
        m_TracePlayer.MoveSpeed = m_TracePlayerSpeed;
        m_TracePlayer.PlayerPos = m_PlayerTr.position;
    }

    public override void SetAlert()
    {
        m_Alert.FOV = m_FOV;
        m_Alert.Agent = m_Agent;
        m_Alert.MoveSpeed = m_AlertSpeed;
    }

    public override void SetAttack()
    {
    }

    public void SetToTraceLight(Transform _flashTr, Vector3 _lightPos)
    {
        m_FlashTr = _flashTr;
        m_LightPos = _lightPos;
    }
    
    public void SetTraceLightToTracePlayer(Vector3 _playerPos)
    {
    }

}
