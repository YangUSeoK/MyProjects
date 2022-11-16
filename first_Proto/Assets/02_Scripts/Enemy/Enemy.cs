using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{


    #region Enemy_Member_variable
    [Header("Speed")]
    [SerializeField] protected float m_PatrolSpeed;
    public float PatrolSpeed
    {
        get { return m_PatrolSpeed; }
    }

    [SerializeField] protected float m_AlertSpeed;
    public float AlertSpeed
    {
        get { return m_AlertSpeed; }
    }

    [SerializeField] protected float m_TracePlayerSpeed;
    public float TracePlayerSpeed
    {
        get { return m_TracePlayerSpeed; }
    }

    [Space]
    [Header("Range")]
    [SerializeField] protected float m_PatrolDetectRange = 20f;
    public float PatrolDetectRange
    {
        get { return m_PatrolDetectRange; }
    }

    [SerializeField] protected float m_PatrolPlayerDetectRange = 10f;
    public float PatrolPlayerDetectRange
    {
        get { return m_PatrolPlayerDetectRange; }
    }

    [SerializeField] protected float m_AlertDetectRange = 15f;
    public float AlertDetectRange
    {
        get { return m_AlertDetectRange; }
    }

    [SerializeField] protected float m_TraceDetectRange = 17f;
    public float TraceDetectRange
    {
        get { return m_TraceDetectRange; }
    }

    [SerializeField] protected float m_AttackRange = 1f;
    public float AttackRange
    {
        get { return m_AttackRange; }
    }

    [Space]
    [Header("Detect Angle")]
    [SerializeField] protected float m_PatrolDetectAngle = 120f;
    public float PatrolDetectAngle
    {
        get { return m_PatrolDetectAngle; }
    }

    [SerializeField] protected float m_AlertDetectAngle = 270f;
    public float AlertDetectAngle
    {
        get { return m_AlertDetectAngle; }
    }

    [SerializeField] protected float m_TraceDetectAngle = 180f;
    public float TraceDetectAngle
    {
        get { return m_TraceDetectAngle; }
    }


    [Space]
    [Header ("EX")]
    [SerializeField] protected Transform m_PlayerTr = null; // EnemyManager가 생성할때 플레이어 먹이면 됨.
    public Transform PlayerTr
    {
        get { return m_PlayerTr; }
    }

    protected NavMeshAgent m_Agent;
    public NavMeshAgent Agent
    {
        get { return m_Agent; }
    }


    #endregion

    #region State
    protected enum EState
    {
        Patrol = 0,
        Alert,
        TracePlayer,
        TraceLight,
        Attack,

        Length
    }
    protected EState mState;


    protected EnemyState m_CurState = null;
    #endregion

    protected virtual void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
    }


    protected void Start()
    {
        m_CurState = GetInitialState();
        if (m_CurState != null)
        {
            m_CurState.EnterState();
        }
    }

    protected void Update()
    {
        m_CurState.CheckState();
        m_CurState.Action();
    }


    // State용
    public void SetState(EnemyState _state)
    {
        if (m_CurState != null)
        {
            m_CurState.ExitState();
        }
        m_CurState = _state;
        m_CurState.EnterState();
    }

    protected virtual EnemyState GetInitialState()
    {
        return null;
    }

    public abstract void SetPatrol();
    public abstract void SetTracePlayer();
    public abstract void SetAlert();
    public abstract void SetAttack();


}
