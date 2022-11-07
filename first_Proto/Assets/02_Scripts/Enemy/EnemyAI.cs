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
    [SerializeField] private EState mState;

    [SerializeField] private Transform m_PlayerTr = null;
    private EnemyFOV mEnemyFOV = null;
    private EnemyMove moveAgent = null;

    
    private float mLookDetectRange = 0f;
    private float mSoundDetectRange = 0f;
    private float mAttackRange = 0f;


    private Vector3 oriPos = Vector3.zero;
    private WaitForSeconds ws = new WaitForSeconds(0.1f);


    private bool mbIsTrace = false;

    private void Awake()
    {
        moveAgent = GetComponent<EnemyMove>();
        mEnemyFOV = GetComponent<EnemyFOV>();

        switch (m_Type) // 디텍팅 거리, 공격사거리 설정하는 곳
        {
            case EType.Light:
                SetRange(10f, 10f, 1f);
                break;

            case EType.Sound:
                SetRange(2f, 20f, 1f);
                oriPos = transform.position;
                break;

            case EType.Wang:
                SetRange(0.5f, 0.5f, 0f);
                break;
        }
    }

    private void SetRange(float _lookDetectRange, float _soundDetectRange, float _attackRange)
    {
        mLookDetectRange = _lookDetectRange;
        mSoundDetectRange = _soundDetectRange;
        mAttackRange = _attackRange;
    }
    private void Start()
    {
        // 현재상황 체크 코루틴 (무한반복)
        StartCoroutine(CheckStateCoroutine());

        // 현재상황에 맞는 액션 코루틴 (무한반복)
        StartCoroutine(ActionCoroutine());
    }



    protected IEnumerator CheckStateCoroutine()
    {
        while (true)
        {
            float dist = Vector3.Distance(m_PlayerTr.position, transform.position);

            // 공격사거리 안쪽이면
            if (dist <= mAttackRange)
            {
                //부채꼴 범위에 있으면 공격한다
                {
                    mState = EState.Attack;
                }
            }

            // 공격사거리 안쪽이 아니면
            else
            {
                // 빛보기맨
                if (m_Type == EType.Light)
                {
                    if (!mbIsTrace)
                    {
                        // 원뿔안에 들어온 상태면 (= 눈에 보이면) 
                        if (mEnemyFOV.GetInCone(mLookDetectRange) && mEnemyFOV.GetIsLookPlayer(mLookDetectRange))
                        {
                            mState = EState.Trace;
                        }

                        // 소리가 들리면
                        else if (dist <= mSoundDetectRange + m_PlayerTr.GetComponent<PlayerMove>().GetNoise())
                        {
                            mState = EState.Alert;
                        }

                        // 아무것도 아니면
                        else
                        {
                            mState = EState.Patrol;
                        }
                    }
                }

                // 소리듣기맨
                if (m_Type == EType.Sound)
                {
                    // if( 소리를 10초안에 1초간격으로 3번 들으면  ) {mState = EState.Tracer}

                    // else if로 바꿔야 함. 소리를 들으면
                    if (dist <= mSoundDetectRange + m_PlayerTr.GetComponent<PlayerMove>().GetNoise())
                    {
                        mState = EState.Alert;
                    }

                    else
                    {
                        mState = EState.Patrol;
                    }
                }

                // 멍멍이
                if (m_Type == EType.Wang)
                {
                    // if(눈으로 플레이어를 발견하면) { mState = EState.Tracer }

                    // else{ mState = EState.Patrol;}
                }
            }













            //// (들을 수 있는 범위 + 발소리 크기)보다 가까이 있으면 || 조명을 본다면      추가해야함 20221026
            //else if (dist <= mSoundDetectRange + m_PlayerTr.GetComponent<PlayerMove>().GetNoise())
            //{
            //    mState = EState.Tracer;
            //    mbIsTracer = true;
            //}

            //else// 아무것도 아니면 Default로 돌아간다.
            //{
            //    if (!mbIsTracer)
            //    {
            //        // switch로 각 종류마다 default 설정
            //        mState = EState.Patrol;
            //    }
            //}

            // 0.1초마다 체크
            yield return ws;
        }
    }
    protected IEnumerator ActionCoroutine()
    {
        while (true)
        {
            yield return ws;

            switch (mState)
            {
                case EState.Patrol:
                    moveAgent.patrollFlags();
                    break;

                case EState.Alert:
                    moveAgent.Stop();
                    moveAgent.SetSpeed((int)mState);
                    moveAgent.TraceTarget(m_PlayerTr.position);
                    Debug.Log("뭔소리야");
                  
                    break;

                case EState.Trace:
                    mbIsTrace = true;   // 한번 추적상태 들어오면 절대 안멈춘다. (숨거나, 죽거나 둘중하나)
                    moveAgent.TraceTarget(m_PlayerTr.position);
                    break;



                // 멍멍이용 코드
                //if (mbIsTracer && !mbCanTraceSet && moveAgent.Agent.remainingDistance <= 2f)
                //{
                //    mbIsTracer = false;
                //    mbCanTraceSet = true;  << 지웠음
                //    mState = EState.Alert;
                //    yield return new WaitForSeconds(2f);
                //}


                case EState.Attack:
                    moveAgent.Stop();
                    mbIsTrace = false;
                    Debug.Log("Die");
                    StopAllCoroutines();
                    // PlayerDie();
                    break;
            }
        }
    }

    public void SetState(string _state)
    {
        mState = (EState)Enum.Parse(typeof(EState), _state);
    }


}
