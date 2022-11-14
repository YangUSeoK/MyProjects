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

        // 20221114 ��켮 : �� �浹��ġ�� �޾ƿ� ��� �����غ���.

        if (colls.Length == 1)
        {
            Vector3 dir = (colls[0].ClosestPoint(transform.position) - transform.position).normalized;

            // 20221114 ��켮 : �������� �غ���     -60 ~ 60 �ؼ� 120��
            if (Vector3.Angle(transform.forward, dir) < m_Angle * 0.5f)
            {
                Debug.Log("���� �ȿ� �����");
                isInFOV = true;
            }
        }
        return isInFOV;
    }
}
