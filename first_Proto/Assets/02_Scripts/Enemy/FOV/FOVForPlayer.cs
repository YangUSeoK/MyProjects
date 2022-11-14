using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVForPlayer : FOV
{
    public override bool IsInFOV()
    {
        bool isInFOV = false;
        Collider[] colls = new Collider[1];
        Physics.OverlapSphereNonAlloc(transform.position, m_Range, colls, 1 << playerLayer);

        if (colls.Length == 1)
        {
            Vector3 dir = (m_TargetTr.position - transform.position).normalized;

            // 20221114 양우석 : 내적으로 해보기     -60 ~ 60 해서 120도
            if (Vector3.Angle(transform.forward, dir) < m_Angle * 0.5f)
            {
                Debug.Log("원뿔 안에 검출됨");
                isInFOV = true;
            }
        }
        return isInFOV;
    }
}
