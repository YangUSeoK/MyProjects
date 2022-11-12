using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyAI : MonoBehaviour
{
    private enum EType
    {
        Light = 0,
        Sound,
        Wang,

        Length
    }
    [SerializeField] private EType m_Type;

    protected enum EState
    {
        Patrol = 0,
        Alert,
        Trace,
        Attack,

        Length
    }
    // 디버그용
    [SerializeField] private EState mState;

    [SerializeField] private Transform m_PlayerTr = null;


    private EnemyState[] m_EnemyStates;
    private EnemyState m_CurState;


    
    private WaitForSeconds ws = new WaitForSeconds(0.1f);



    private void Awake()
    {
        m_EnemyStates = new EnemyState[4];
        m_EnemyStates[(int)EState.Patrol] = new Patrol(this);
        m_EnemyStates[(int)EState.Alert] = new Alert(this);
        m_EnemyStates[(int)EState.Trace] = new Trace(this);
        m_EnemyStates[(int)EState.Attack] = new Attack(this);
    }

    
    private void Start()
    {
        SetState(m_EnemyStates[0]);
    }

    private void Update()
    {
        m_CurState.UpdateLogic(this);
    }




    public void SetState(EnemyState _state)
    {
        if (m_CurState != null)
        {
            m_CurState.ExitState(this);
        }
            m_CurState = _state;
            m_CurState.EnterState(this);
    }


}
