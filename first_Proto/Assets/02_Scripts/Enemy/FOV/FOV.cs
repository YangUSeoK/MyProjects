using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ���� �տ� �÷��̾ �ɷȴ��� Ȯ���ϴ� ��ũ��Ʈ
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
        
        Debug.Log("���� �����");

        // 20221114 ��켮 : ���̽�� ��ġ ���񿡵��� �����ؾ� ��
        if(Physics.Raycast(transform.position + transform.forward, dir, out hitInfo, m_Range, layerMask))
        {
            Debug.Log("���� �¾���!");
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
