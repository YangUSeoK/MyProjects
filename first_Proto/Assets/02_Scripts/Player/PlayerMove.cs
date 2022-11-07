using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private enum EState
    {
        Normal = 0,
        Slow,
        Run,
        Hide,
        Die,

        length
    }
    [SerializeField] private EState mState;


    private float mRunSpeed = 5f;
    private float mNormalSpeed = 4f;
    private float mSlowSpeed = 1f;
    private float mCurSpeed = 0f;
    private float mMakeNoise = 0f;
    private float mRotSpeed = 100f;
    private float mYRotate = 0f;

    private float mAxisV = 0f;
    private float mAxisH = 0f;
    private float mAxisY = 0f;
    




    private void Update()
    {
        mAxisV = Input.GetAxis("Vertical");
        mAxisH = Input.GetAxis("Horizontal");

        Vector3 dir = new Vector3(mAxisH, 0f, mAxisV);

        //dir = Camera.main.transform.TransformDirection(dir);


        // 왼쪽시프트 누르면 달리기
        if (Input.GetKey(KeyCode.LeftShift))
        {
            mState = EState.Run;
        }
        // 왼쪽컨트롤 누르면걷기
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            mState = EState.Slow;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            mState = EState.Hide;
        }
        // 기본 : 걷기상태
        else
        {
            mState = EState.Normal;
        }
        
        // 현재 상태에 따른 변수 설정
        switch (mState)
        {
            case EState.Normal:
                SetValueOnState(mNormalSpeed, 3f);
                break;
            case EState.Slow:
                SetValueOnState(mSlowSpeed, 1f);
                break;
            case EState.Run:
                SetValueOnState(mRunSpeed, 10f);

                break;
            case EState.Hide:
                SetValueOnState(0f, 0f);
                break;
            default:
                SetValueOnState(0f, 0f);
                UnityEngine.Debug.Log("Player State Error");
                break;
        }

        transform.Translate(dir * mCurSpeed * Time.deltaTime);

        mAxisY = Input.GetAxis("Mouse X");
        mYRotate = transform.eulerAngles.y + (mAxisY * mRotSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(0f, mYRotate, 0f);

    }

    private void SetValueOnState(float _setSpeed, float _setNoise)
    {
        mCurSpeed = _setSpeed;
        mMakeNoise = _setNoise;
    }

    public string GetPlayerState()
    {
        return mState.ToString();
    }
    
    public float GetNoise()
    {
        return mMakeNoise;
    }

}
