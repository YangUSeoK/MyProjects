using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



[CustomEditor(typeof(FOV))]
public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {
        // target은 EnemyFOV에서 콜백함수로 받아옴
        FOV fov = (FOV)target;


        // 하얀색 원을 그림.
        Handles.color = Color.white;
        Handles.DrawWireDisc(fov.transform.position, Vector3.up, fov.m_Range);

        // 반투명 원을 그림(부채꼴, 시야범위 표시할 것)
        Handles.color = new Color(1f, 1f, 1f, 0.2f);

        // enemyFOV에서 원 위의 한 점을 잡아서 부채꼴 시작점으로 지정
        Vector3 fromAnglePos = fov.DirFromAngle(-fov.m_Angle * 0.5f);

        // 부채꼴 그림 -         각도 원점,             노말벡터,       시작점,      부채꼴 각도,    부채꼴 반지름
        Handles.DrawSolidArc(fov.transform.position, Vector3.up, fromAnglePos, fov.m_Angle, fov.m_Range);

        // 라벨 붙임              시작점               시작점 앞으로 2               라벨 내용(string)
        Handles.Label(fov.transform.position + (fov.transform.forward * 2f), fov.m_Angle.ToString());
    }
}
