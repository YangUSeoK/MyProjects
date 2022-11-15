using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 원뿔 범위 앞에 플레이어가 걸렸는지 확인하는 스크립트
public class FOV : MonoBehaviour
{
    // 에디터용
    public float m_Angle = 120f;
    public float m_Range = 10f;


    protected Transform m_TargetTr = null;
    protected int m_PlayerLayer = 0;
    protected int m_ObstacleLayer = 0;
    private int m_LightLayer = 0;
    protected int m_LayerMask = 0;

    protected virtual void Start()
    {
        m_PlayerLayer = LayerMask.NameToLayer("PLAYER");
        m_ObstacleLayer = LayerMask.NameToLayer("OBSTACLE");
        m_LightLayer = LayerMask.NameToLayer("LIGHT");
        m_LayerMask = (1 << m_PlayerLayer) | (1 << m_ObstacleLayer) | (1 << m_LightLayer);
    }

    public bool IsInFOV(float _detectRange, float _angle, int _layerMask)
    {
        bool isInFOV = false;
        // Enemy의 포지션에서부터 viewRange 만큼 구체를 
        // 그려서 그 중에 playerLayer 가 있으면 colls 배열에 추가
        Collider[] colls = new Collider[1];
        Physics.OverlapSphereNonAlloc(transform.position, _detectRange, colls, _layerMask);

        // 플레이어 레이어가 검출되었을 때
        if (colls.Length == 1)
        {
            // A - B 벡터 >> B에서 A를 바라보는 방향 + 정규화
            Vector3 dir = (colls[0].transform.position - transform.position).normalized;

            // 내가 본 전방방향에서 방금 구한 dir방향의 각도가 120도 범위 안에 있으면
            if (Vector3.Angle(transform.forward, dir) < _angle * 0.5f)
            {
                isInFOV = true;
            }
        }
        return isInFOV;
    }

    public bool IsLookDirect(Transform _targetTr, float _detectRange, int _layerMask)
    {
        bool isLook = false;

        RaycastHit hitInfo;
        Vector3 dir = (_targetTr.position - transform.position).normalized;

        Debug.Log("레이 쏘는중");

        // 20221114 양우석 : 레이쏘는 위치 좀비에따라 보정해야 함
        if (Physics.Raycast(transform.position + transform.forward, dir, out hitInfo, _detectRange, m_LayerMask))
        {
            isLook = hitInfo.collider.tag == _targetTr.tag;

            if (isLook) Debug.Log("레이 맞았음!");
        }
        return isLook;
    }

    // 부채꼴로 레이를 쏴서 부딪히면 위치를 저장한다.
    public bool IsInDirectFovWithRay(float _detectRange, float _angle, int _layerMask, ref Vector3 _collPos)
    {
        bool isInDirectFOV = false;
        int stepCnt = 20;
        float stepAngleSize = _angle / stepCnt;

        for (int i = 0; i <= stepCnt; ++i)
        {
            float rayAngle = ((_angle / 2) + (stepAngleSize * i)) - _angle ;
            Vector3 dir = DirFromAngle(rayAngle);

            Debug.DrawLine(transform.position, transform.position + (dir * _detectRange), Color.green);


            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, dir, out hitInfo, _detectRange, m_LayerMask))
            {
                Debug.Log("좀비가 부채꼴 레이를 쏜다");

                isInDirectFOV = hitInfo.collider.CompareTag("LIGHT");
                if (isInDirectFOV)
                {
                    Debug.Log("좀비가 빛을 발견했다!");
                    _collPos = hitInfo.point;
                    break;
                }
            }
        }
        return isInDirectFOV;
    }


    public Vector3 DirFromAngle(float _angleDegree)
    {
        _angleDegree += transform.eulerAngles.y;
        return new Vector3(Mathf.Cos((-_angleDegree + 90f) * Mathf.Deg2Rad),
                            0f,
                            Mathf.Sin((-_angleDegree + 90f) * Mathf.Deg2Rad)).normalized;
    }

    // 각도를 수치로 입력받으면 내 현재 각도에서 그만큼 회전한 방향의 방향벡터를 반환한다. : 기준벡터
    private Vector3 DirFromAngle(float _angleDegree, float _verticalAngleDegree)
    {

        _angleDegree += transform.eulerAngles.y;
        _verticalAngleDegree += transform.eulerAngles.x;

        // 20221108 양우석 : 해결했음.
        return new Vector3(Mathf.Cos((-_angleDegree + 90f) * Mathf.Deg2Rad),
                            Mathf.Tan(-_verticalAngleDegree * Mathf.Deg2Rad),
                            Mathf.Sin((-_angleDegree + 90f) * Mathf.Deg2Rad)).normalized;
    }
}
