using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private FlagManager m_FlagManager = null;
    private Flag[] mFlags;
    private int mNextIdx = 0;

    public NavMeshAgent Agent = null;
    private float mCurSpeed = 3f;


    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Agent.autoBraking = false;
        Agent.speed = mCurSpeed;

        
    }

    private void Start()
    {
        mFlags = m_FlagManager.Flags;
        patrollFlags();
    }

    private void Update()
    {
        // ���� �� �� ���
        if(Agent.remainingDistance <= 0.5f)
        {
            // �ε����� ����ȣ���� ���� �ٽ� ó������. %�� ������
            ++mNextIdx;
            if(mNextIdx >= mFlags.Length)
            {
                mNextIdx -= mFlags.Length;
            }
            patrollFlags();
        }
    }

    public void Stop()
    {
        Agent.isStopped = true;
        Agent.velocity = Vector3.zero;
    }

    public void SetSpeed(int _state)
    {
        switch (_state)
        {
            case 0: // Patrol
                mCurSpeed = 1.5f;
                break;
            case 1: // Alert
                mCurSpeed = 1f;
                break;
            case 2: // Trace
                mCurSpeed = 5f;
                break;
            case 3:
                mCurSpeed = 0f;
                break;
            default: // ������
                mCurSpeed = 0f;
                break;
        }
    }

    public void TraceTarget(Vector3 _pos)
    {
        if (Agent.isPathStale)
        {
            return;
        }

            Agent.destination = _pos;
            Agent.isStopped = false;


        
    }

    public void patrollFlags()
    {
        // ��� ������� ���� ����
        if (Agent.isPathStale)
        {
            return;
        }
        Agent.destination = mFlags[mNextIdx].transform.position;
        Agent.isStopped = false;
    }
}

