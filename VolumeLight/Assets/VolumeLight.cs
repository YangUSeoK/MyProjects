using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VolumeLight : MonoBehaviour
{

    // Ray ����
    private Light m_Light = null;
    private float m_LightRange = 0f;

    // ���̸� �ɰ��� ����. Ŭ���� �߰� �ɰ���
    [SerializeField] private float m_MeshRate = 0.1f;


    // Mesh ��������. 20221106 ��켮 : ���߿� Ŭ���� �ɰ� ���� ����.
    private Mesh mLightMesh = null;
    private MeshCollider m_MeshCollider = null;


    // ray�� ������ �����ϴ� ����ü
    private struct RaycastInfo
    {
        public bool isHit;
        public Vector3 pointPos;
        public float distance;
        public float angle;

        // ������
        public RaycastInfo(bool _isHit, Vector3 _pointPos, float _distance, float _angle)
        {
            isHit = _isHit;
            pointPos = _pointPos;
            distance = _distance;
            angle = _angle;
        }
    }

    private void Awake()
    {
        m_Light = GetComponent<Light>();

        // ������Ƽ�� ��� �����ϴ°� �Ⱦ ������ ����.
        m_LightRange = m_Light.range;

        mLightMesh = new Mesh();
        mLightMesh.name = "LightMesh";
        GetComponentInChildren<MeshFilter>().mesh = mLightMesh;
        m_MeshCollider = GetComponentInChildren<MeshCollider>();
        m_MeshCollider.sharedMesh = mLightMesh;
    }

    private void LateUpdate()
    {
        DrawFOV();
    }

    private void DrawFOV()
    {
        // ���� * ���� �ݿø�
        int stepCnt = Mathf.RoundToInt(m_Light.spotAngle * m_MeshRate);

        // �ٽ� ���� ������ ������ �ɰ����� �ޱ��� ���� ����
        float stepAngleSize = m_Light.spotAngle / stepCnt;

        List<Vector3> lightPointList = new List<Vector3>();

        // ��ä�÷� ���̸� ���� ����ü�� ������ �����Ѵ�.
        for (int i = 0; i <= stepCnt; ++i)
        {
            float angle = transform.eulerAngles.y - (m_Light.spotAngle / 2) + (stepAngleSize * i);

            // ����׿� DrawLine
            //Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle, true) * m_LightRange, Color.green);

            RaycastInfo newRayCastInfo = SetRaycastInfo(angle);
            lightPointList.Add(newRayCastInfo.pointPos);
        }


        // �Ʒ����ʹ� �޽� ����� �κ�


        // 20221106 ��켮 : �޽� ����� �Լ��� ���� �ɰ� ��
        // +1 ���ִ� ���� : �� ó�� ������ ������ �߰������ �Ѵ�.
        int vertexCnt = lightPointList.Count + 1;
        Vector3[] vertices = new Vector3[vertexCnt];



        // �ﰢ�� ���� : vertexCnt-2
        // �ﰢ�� �������� �� : �ﰢ�� ���� * 3
        int[] triangles = new int[(vertexCnt - 2) * 3];

        // ����Ƽ �޽ô� ������ǥ�� �����̴�.
        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexCnt - 1; ++i)
        {
            // InverseTransformPoint : ������ǥ ��ġ �޾ƿ���.
            vertices[i + 1] = transform.InverseTransformPoint(lightPointList[i]);

            // �迭�������� ����
            if (i < vertexCnt - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        mLightMesh.Clear();
        mLightMesh.vertices = vertices;
        mLightMesh.triangles = triangles;
        mLightMesh.RecalculateNormals();
        

        // �ݶ��̴��� aabb tree�� ����ϱ� ������, �޽��� ����Ǹ� �ٽ� �������־�� �Ѵ�.
        m_MeshCollider.enabled = false;
        m_MeshCollider.enabled = true;

        
    }


    // tarnsform�� Euler.y ���� ���ڷ� �޾ƿͼ�, �� �������� Ray�� ���� hit ������ ����ü�� �����Ѵ�.
    private RaycastInfo SetRaycastInfo(float _angleDegree)
    {
        Vector3 dir = DirFromAngle(_angleDegree, transform.eulerAngles.x, true);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, m_LightRange))
        {
            return new RaycastInfo(true, hit.point, hit.distance, _angleDegree);
        }
        else
        {
            return new RaycastInfo(false, transform.position + (dir * m_LightRange), m_LightRange, _angleDegree);
        }
    }

    // ������ ��ġ�� �Է¹����� �� ���� �������� �׸�ŭ ȸ���� ������ ���⺤�͸� ��ȯ�Ѵ�. : ���غ���
    private Vector3 DirFromAngle(float _angleDegree, float _verticalAngleDegree, bool _angleIsGlobal)
    {
        if (!_angleIsGlobal)
        {
            _angleDegree += transform.eulerAngles.y;
            _verticalAngleDegree += transform.eulerAngles.x;
        }
        
        
        // 20221108 ��켮 : �ذ�����.
        return new Vector3(Mathf.Cos((-_angleDegree + 90f) * Mathf.Deg2Rad), 
                            Mathf.Tan(-_verticalAngleDegree * Mathf.Deg2Rad), 
                            Mathf.Sin((-_angleDegree + 90f) * Mathf.Deg2Rad)).normalized;
    }


}
