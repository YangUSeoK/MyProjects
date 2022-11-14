using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    // 20221114 양우석 : EnemyManager에서 먹여야 함
    [SerializeField] private FlagManager m_FlagManager = null;
    private Flag[] mFlags;
    private int mNextIdx = 0;

    public NavMeshAgent Agent = null;
    public float MoveSpeed
    {
        set { Agent.speed = value; }
    }

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Agent.autoBraking = false;
    }

    private void Start()
    {
        mFlags = m_FlagManager.Flags;
        patrollFlags();
    }

    private void Update()
    {
        // 다음 갈 곳 계산
        if (Agent.remainingDistance <= 0.5f)
        {
            // 인덱스가 끝번호까지 가면 다시 처음으로. %랑 같은거
            ++mNextIdx;
            if (mNextIdx >= mFlags.Length)
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

