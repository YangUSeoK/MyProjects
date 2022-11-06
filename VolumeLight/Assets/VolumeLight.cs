using System.Collections;
using System.Collections.Generic;
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
    private MeshFilter mLightMeshFilter = null;


    private void Awake()
    {
        m_Light = GetComponent<Light>();
        m_LightRange = m_Light.range;

        mLightMesh = new Mesh();
        mLightMesh.name = "LightMesh";
        mLightMeshFilter = GetComponentInChildren<MeshFilter>();
        mLightMeshFilter.mesh = mLightMesh;
    }


    private void LateUpdate()
    {
        DrawFOV();
    }


    // ray�� ������ �����ϴ� ����ü
    public struct RaycastInfo
    {
        public bool hit;
        public Vector3 pointPos;
        public float range;
        public float angle;

        // ������
        public RaycastInfo(bool _hit, Vector3 _pointPos, float _range, float _angle)
        {
            hit = _hit;
            pointPos = _pointPos;
            range = _range;
            angle = _angle;
        }
    }

    // tarnsform�� Euler.y ���� ���ڷ� �޾ƿͼ�, �� �������� Ray�� ���� hit ������ ����ü�� �����Ѵ�.
    private RaycastInfo SetRaycastInfo(float _angleDegree)
    {
        Vector3 dir = DirFromAngle(_angleDegree, true);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, m_LightRange))
        {
            return new RaycastInfo(true, hit.point, hit.distance, _angleDegree);
        }
        else
        {
            return new RaycastInfo(false, m_LightRange * transform.position + dir , m_LightRange, _angleDegree);
        }
    }

    // ������ ��ġ�� �Է¹����� �� ���� �������� �׸�ŭ ȸ���� ������ ���⺤�͸� ��ȯ�Ѵ�.
    public Vector3 DirFromAngle(float _angleDegree, bool _angleIsGlobal)
    {
        if (!_angleIsGlobal)
        {
            _angleDegree += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Cos((-_angleDegree + 90) * Mathf.Deg2Rad), 0, Mathf.Sin((-_angleDegree + 90) * Mathf.Deg2Rad)).normalized;
    }

    private void DrawFOV()
    {
        // ���� * ���� �ݿø�
        int stepCnt = Mathf.RoundToInt(m_Light.spotAngle * m_MeshRate);

        // �ٽ� ���� ������ ������ �ɰ����� �ޱ��� ���� ����
        float stepAngleSize = m_Light.spotAngle / stepCnt;

        List<Vector3> lightPointList = new List<Vector3>();

        for(int i = 0; i <= stepCnt; ++i)
        {
            float angle = transform.eulerAngles.y - (m_Light.spotAngle / 2) + (stepAngleSize * i);

            // ����׿� DrawLine
            //Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle, true) * m_Light.spotAngle, Color.green);

            RaycastInfo newRayCastInfo = SetRaycastInfo(angle);
            lightPointList.Add(newRayCastInfo.pointPos);
        }

        // 20221106 ��켮 : �޽� ����� �Լ��� ���� �ɰ� ��
        // +1 ���ִ� ���� : �� ó�� ������ ������ �߰������ �Ѵ�.
        int vertexCnt = lightPointList.Count + 1;
        Vector3[] vertices = new Vector3[vertexCnt];

        // 20221106 ��켮 : �̺κ� �迭�������� ������ �� ����. ���� �� ��
        int[] triangles = new int[(vertexCnt - 2) * 3]; // �ﰢ�� ���� vertexCnt-2  ������ ���� : �ﰢ������ *3

        // ����Ƽ �޽ô� ������ǥ�� �����̴�.
        vertices[0] = Vector3.zero;

        for(int i = 0; i < vertexCnt-1; ++i)
        {
            vertices[i+1] = transform.InverseTransformPoint(lightPointList[i]);

            // �迭�������� ����
            if(i<vertexCnt - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i+ 1;
                triangles[i * 3 + 2] = i+ 2;
            }
        }

        mLightMesh.Clear();
        mLightMesh.vertices = vertices;
        mLightMesh.triangles = triangles;
        mLightMesh.RecalculateNormals();

    }



    
}
