using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 원뿔 범위 앞에 플레이어가 걸렸는지 확인하는 스크립트
public abstract class FOV : MonoBehaviour
{
    protected Transform m_TargetTr = null;
    public float m_Angle;
    public float m_Range;

    

    protected int playerLayer = 0;
    protected int obstacleLayer = 0;
    protected int layerMask = 0;

    protected virtual void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        obstacleLayer = LayerMask.NameToLayer("Obstacle");
        layerMask = (1 << playerLayer) | (1 << obstacleLayer);
    }

    public abstract bool IsInFOV();

    public bool IsLookDirect()
    {
        bool isLook = false;

        RaycastHit hitInfo;
        Vector3 dir = (m_TargetTr.position - transform.position).normalized;
        
        Debug.Log("레이 쏘는중");

        // 20221114 양우석 : 레이쏘는 위치 좀비에따라 보정해야 함
        if(Physics.Raycast(transform.position + transform.forward, dir, out hitInfo, m_Range, layerMask))
        {
            Debug.Log("레이 맞았음!");
            isLook = hitInfo.collider.CompareTag("Player");
        }

        return isLook;
    }

    public void SetFOV(Transform _targetTr, float _range, float _angle)
    {
        Debug.Log("setFOV");
        m_TargetTr = _targetTr;
        m_Range = _range;
        m_Angle = _angle;
    }


    public Vector3 CirclePoint(float angle)
    {
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
