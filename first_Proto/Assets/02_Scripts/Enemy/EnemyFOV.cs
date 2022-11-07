using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 원뿔 범위 앞에 플레이어가 걸렸는지 확인하는 스크립트
public class EnemyFOV : MonoBehaviour
{
    [SerializeField] private Transform m_PlayerTr = null;

    public float viewAngle = 120f;
    public float viewRange = 0f;


    
    private int playerLayer = 0;
    private int obstacleLayer = 0;
    private int layerMask = 0;

    private void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
        obstacleLayer = LayerMask.NameToLayer("Obstacle");
        layerMask = (1 << playerLayer) | (1 << obstacleLayer);
    }



    public bool GetInCone(float _detectRange)
    {
        bool isInCone = false;

        Collider[] colls = Physics.OverlapSphere(transform.position, _detectRange, 1 << playerLayer);
        viewRange = _detectRange;

        if(colls.Length == 1)
        {
            Vector3 dir = (m_PlayerTr.position - transform.position).normalized;

            //                                          -60 ~ 60 해서 120도
            if (Vector3.Angle(transform.forward, dir) < viewAngle * 0.5f)
            {
                Debug.Log("원뿔 안에 검출됨");
                isInCone = true;
            }
        }
        return isInCone;
    }

    public bool GetIsLookPlayer(float _detectRange)
    {
        bool isLook = false;

        RaycastHit hitInfo;
        Vector3 dir = (m_PlayerTr.position - transform.position).normalized;
        
        Debug.Log("레이 쏘는중");

        if(Physics.Raycast(transform.position + transform.forward, dir, out hitInfo, _detectRange, layerMask))
        {
            Debug.Log("레이 맞았음!");
            isLook = hitInfo.collider.CompareTag("Player");
        }

        return isLook;
    }

    public Vector3 CirclePoint(float angle)
    {
        angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

}
