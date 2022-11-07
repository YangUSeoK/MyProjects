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

        switch (m_Type) // ������ �Ÿ�, ���ݻ�Ÿ� �����ϴ� ��
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
        // �����Ȳ üũ �ڷ�ƾ (���ѹݺ�)
        StartCoroutine(CheckStateCoroutine());

        // �����Ȳ�� �´� �׼� �ڷ�ƾ (���ѹݺ�)
        StartCoroutine(ActionCoroutine());
    }



    protected IEnumerator CheckStateCoroutine()
    {
        while (true)
        {
            float dist = Vector3.Distance(m_PlayerTr.position, transform.position);

            // ���ݻ�Ÿ� �����̸�
            if (dist <= mAttackRange)
            {
                //��ä�� ������ ������ �����Ѵ�
                {
                    mState = EState.Attack;
                }
            }

            // ���ݻ�Ÿ� ������ �ƴϸ�
            else
            {
                // �������
                if (m_Type == EType.Light)
                {
                    if (!mbIsTrace)
                    {
                        // ���Ծȿ� ���� ���¸� (= ���� ���̸�) 
                        if (mEnemyFOV.GetInCone(mLookDetectRange) && mEnemyFOV.GetIsLookPlayer(mLookDetectRange))
                        {
                            mState = EState.Trace;
                        }

                        // �Ҹ��� �鸮��
                        else if (dist <= mSoundDetectRange + m_PlayerTr.GetComponent<PlayerMove>().GetNoise())
                        {
                            mState = EState.Alert;
                        }

                        // �ƹ��͵� �ƴϸ�
                        else
                        {
                            mState = EState.Patrol;
                        }
                    }
                }

                // �Ҹ�����
                if (m_Type == EType.Sound)
                {
                    // if( �Ҹ��� 10�ʾȿ� 1�ʰ������� 3�� ������  ) {mState = EState.Tracer}

                    // else if�� �ٲ�� ��. �Ҹ��� ������
                    if (dist <= mSoundDetectRange + m_PlayerTr.GetComponent<PlayerMove>().GetNoise())
                    {
                        mState = EState.Alert;
                    }

                    else
                    {
                        mState = EState.Patrol;
                    }
                }

                // �۸���
                if (m_Type == EType.Wang)
                {
                    // if(������ �÷��̾ �߰��ϸ�) { mState = EState.Tracer }

                    // else{ mState = EState.Patrol;}
                }
            }













            //// (���� �� �ִ� ���� + �߼Ҹ� ũ��)���� ������ ������ || ������ ���ٸ�      �߰��ؾ��� 20221026
            //else if (dist <= mSoundDetectRange + m_PlayerTr.GetComponent<PlayerMove>().GetNoise())
            //{
            //    mState = EState.Tracer;
            //    mbIsTracer = true;
            //}

            //else// �ƹ��͵� �ƴϸ� Default�� ���ư���.
            //{
            //    if (!mbIsTracer)
            //    {
            //        // switch�� �� �������� default ����
            //        mState = EState.Patrol;
            //    }
            //}

            // 0.1�ʸ��� üũ
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
                    Debug.Log("���Ҹ���");
                  
                    break;

                case EState.Trace:
                    mbIsTrace = true;   // �ѹ� �������� ������ ���� �ȸ����. (���ų�, �װų� �����ϳ�)
                    moveAgent.TraceTarget(m_PlayerTr.position);
                    break;



                // �۸��̿� �ڵ�
                //if (mbIsTracer && !mbCanTraceSet && moveAgent.Agent.remainingDistance <= 2f)
                //{
                //    mbIsTracer = false;
                //    mbCanTraceSet = true;  << ������
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
