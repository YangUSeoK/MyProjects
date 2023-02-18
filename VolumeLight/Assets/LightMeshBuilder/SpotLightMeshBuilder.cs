using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpotLightMeshBuilder : MonoBehaviour
{
    // ���̸� �ɰ��� ����. Ŭ���� �߰� �ɰ���
    [SerializeField, Range(10f, 100f)] private int m_SubDivision = 100;
    public int SubDivision
    { get { return m_SubDivision; } }
    private Light m_Light = null;
    private Vector3[] m_LightVerts = null;

    // Mesh ��������. 
    private Mesh m_LightMesh = null;
    private MeshCollider m_MeshCollider = null;

    public float radius;

    private void Awake()
    {
        m_LightMesh = new Mesh();
        m_Light = GetComponent<Light>();
        m_LightMesh.name = "LightMesh";
        GetComponentInChildren<MeshFilter>().mesh = m_LightMesh;
        m_MeshCollider = GetComponentInChildren<MeshCollider>();
        m_MeshCollider.sharedMesh = m_LightMesh;
    }

    private void LateUpdate()
    {
        SetConeMeshPoints();
        BuildConeMesh(m_LightVerts);
    }

    private void SetConeMeshPoints()
    {
        if (m_SubDivision <= 0) return;
        if (m_Light.range <= 0f) return;
        if (m_Light.spotAngle <= 0f) return;

        float n = 1f / (m_SubDivision);     // �������� �����ϱ� ������ �޾Ƶд�.
        float radius = Mathf.Tan((m_Light.spotAngle * 0.5f) * Mathf.Deg2Rad);
        this.radius = radius;
        float length = m_Light.range / Mathf.Cos(m_Light.spotAngle * 0.5f * Mathf.Deg2Rad);
        Vector3 origCircleVert = Vector3.zero;
        Vector3 newCircleVert = Vector3.zero;

        m_LightVerts = new Vector3[m_SubDivision];

        for (int i = 0; i < m_SubDivision; ++i)
        {
            float ratio = ((float)i) * n;            // ���� SubDivison���� ���� �� ���°�� ��ȣ����
            float theta = (Mathf.PI * 2f) * ratio;   // ���� * ���� = ����. ration�� radian���� ǥ��
            float x = Mathf.Cos(theta) * radius;
            float y = Mathf.Sin(theta) * radius;

            Debug.Log(Mathf.Cos(theta));

            origCircleVert = new Vector3(x, y, 1); //>> z�� �߽����� ���� ���� ��ǥ

            // ȸ������� ���� origCircleVert�� �� ���� ȸ����Ų��.
            Quaternion rotation = Quaternion.Euler(this.transform.eulerAngles.x,
                                                   this.transform.eulerAngles.y,
                                                   this.transform.eulerAngles.z);
            Matrix4x4 rotMatrix = Matrix4x4.Rotate(rotation);
            newCircleVert = rotMatrix.MultiplyPoint3x4(origCircleVert);

            // ȸ���� newCircleVert�� ��ġ�� ���� length ���̸�ŭ ���̸� ����,
            // �ε��� ���� ������ lightVertlist�� �����Ѵ�.
            Vector3 raycastPoint = SetRaycastPoint(newCircleVert, length);

            m_LightVerts[i] = raycastPoint;
        }
    }


    // lightVerts�� �޾Ƽ� �޽��� ������ش�.
    private void BuildConeMesh(Vector3[] _lightVerts)
    {
        if (_lightVerts.Length <= 1) return;

        // +1 ���ִ� ���� : �� ó�� ������ ������ �߰������ �Ѵ�.
        Vector3[] vertices = new Vector3[_lightVerts.Length + 1];

        // �ﰢ�� ���� : vertexCnt-1
        // �ﰢ�� �������� �� : �ﰢ�� ���� * 3
        int[] triangles = new int[_lightVerts.Length * 3];

        // ����Ƽ �޽ô� ������ǥ�� �����̴�.
        vertices[0] = Vector3.zero;

        for (int i = 0; i < _lightVerts.Length; ++i)
        {
            // InverseTransformPoint : ������ǥ ��ġ �޾ƿ���.
            vertices[i + 1] = transform.InverseTransformPoint(_lightVerts[i]);

            // triangle �迭�� ������� ����
            if (i < _lightVerts.Length-1)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }
        // ������, ��������, ó������ ������ �ﰢ������ �������ش�.
        triangles[(_lightVerts.Length - 1) * 3] = 0;
        triangles[(_lightVerts.Length - 1) * 3 + 1] = _lightVerts.Length;
        triangles[(_lightVerts.Length - 1) * 3 + 2] = 1;
         

        m_LightMesh.Clear();
        m_LightMesh.vertices = vertices;
        m_LightMesh.triangles = triangles;
        m_LightMesh.RecalculateNormals();    // �븻 �޾��ֱ�

        // �ݶ��̴��� aabb tree�� ����ϱ� ������, �޽��� ����Ǹ� �ٽ� �������־�� �Ѵ�.
        m_MeshCollider.enabled = false;
        m_MeshCollider.enabled = true;
    }

    // ���⺤�Ϳ� ���̸� �޾ƿͼ� ���̸� ��� point�� ��ȯ�Ѵ�.
    private Vector3 SetRaycastPoint(Vector3 _dir, float _length)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(this.transform.position, _dir, out hitInfo, _length))
        {
            return hitInfo.point;
        }
        else
        {
            return this.transform.position + (_dir * _length);
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
