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
        // 다음 갈 곳 계산
        if(Agent.remainingDistance <= 0.5f)
        {
            // 인덱스가 끝번호까지 가면 다시 처음으로. %랑 같은거
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
            default: // 나머지
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
        // 경로 계산중일 때는 리턴
        if (Agent.isPathStale)
        {
            return;
        }
        Agent.destination = mFlags[mNextIdx].transform.position;
        Agent.isStopped = false;
    }
}

