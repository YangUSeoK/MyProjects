using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVForLight : FOV
{
    public override bool IsInFOV()
    {
        bool isInFOV = false;
        Collider[] colls = new Collider[3];
        Physics.OverlapSphereNonAlloc(transform.position, m_Range, colls, 1 << playerLayer);

        // 20221114 양우석 : 빛 충돌위치를 받아올 방법 생각해보기.

        if (colls.Length == 1)
        {
            Vector3 dir = (colls[0].ClosestPoint(transform.position) - transform.position).normalized;

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
