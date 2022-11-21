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

    protected NavMeshAgent m_Agent;
    public NavMeshAgent Agent
    {
        set { m_Agent = value; }
    }
    public float MoveSpeed
    {
        set { m_Agent.speed = value; }
    }

    protected FOV m_FOV;
    public FOV FOV
    {
        set
        {
            if (m_FOV == null)
            {
                m_FOV = value;
            }
        }
    }



    public abstract void EnterState();
    public abstract void CheckState();
    public abstract void Action();
    public abstract void ExitState();
}
