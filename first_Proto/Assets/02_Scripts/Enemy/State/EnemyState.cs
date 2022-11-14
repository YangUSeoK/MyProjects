using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyState
{

    protected Enemy m_Enemy;
    protected string m_Name;
    public string Name
    {
        get { return m_Name; }
    }

    public EnemyState(string _name, Enemy _enemy)
    {
        m_Name = _name;
        m_Enemy = _enemy;
    }

    protected FOVForPlayer m_FOVForPlayer;
    public FOVForPlayer FOVForPlayer
    {
        set
        {
            if (m_FOVForPlayer == null)
            {
                m_FOVForPlayer = value;
            }
        }
    }
    protected FOVForLight m_FOVForLight;
    public FOVForLight FOVForLight
    {
        set
        {
            if (m_FOVForLight == null)
            {
                m_FOVForLight = value;
            }
        }
    }

    protected NavMeshAgent m_Agent;
    public NavMeshAgent Agent
    {
        set { m_Agent = value; }
    }
    public float MoveSpeed
    {
        set { m_Agent.speed = value; }
    }


    public abstract void EnterState();
    public abstract void CheckState();
    public abstract void Action();
    public abstract void ExitState();
}
