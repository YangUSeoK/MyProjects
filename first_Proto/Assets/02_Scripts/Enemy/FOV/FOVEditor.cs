using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



[CustomEditor(typeof(FOVForPlayer))]
public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        // target�� EnemyFOV���� �ݹ��Լ��� �޾ƿ�
        FOVForPlayer fov = (FOVForPlayer)target;


        // �Ͼ�� ���� �׸�.
        Handles.color = Color.white;
        Handles.DrawWireDisc(fov.transform.position, Vector3.up, fov.m_Range);

        // ������ ���� �׸�(��ä��, �þ߹��� ǥ���� ��)
        Handles.color = new Color(1f, 1f, 1f, 0.2f);

        // enemyFOV���� �� ���� �� ���� ��Ƽ� ��ä�� ���������� ����
        Vector3 fromAnglePos = fov.CirclePoint(-fov.m_Angle * 0.5f);

        // ��ä�� �׸� -         ���� ����,             �븻����,       ������,      ��ä�� ����,    ��ä�� ������
        Handles.DrawSolidArc(fov.transform.position, Vector3.up, fromAnglePos, fov.m_Angle, fov.m_Range);

        // �� ����              ������               ������ ������ 2               �� ����(string)
        Handles.Label(fov.transform.position + (fov.transform.forward * 2f), fov.m_Angle.ToString());
    }
}
