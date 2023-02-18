using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpotLightMeshBuilder : MonoBehaviour
{
    // 레이를 쪼개는 비율. 클수록 잘게 쪼갠다
    [SerializeField, Range(10f, 100f)] private int m_SubDivision = 100;
    public int SubDivision
    { get { return m_SubDivision; } }
    private Light m_Light = null;
    private Vector3[] m_LightVerts = null;

    // Mesh 생성관련. 
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

        float n = 1f / (m_SubDivision);     // 나눗셈은 느리니까 변수에 받아둔다.
        float radius = Mathf.Tan((m_Light.spotAngle * 0.5f) * Mathf.Deg2Rad);
        this.radius = radius;
        float length = m_Light.range / Mathf.Cos(m_Light.spotAngle * 0.5f * Mathf.Deg2Rad);
        Vector3 origCircleVert = Vector3.zero;
        Vector3 newCircleVert = Vector3.zero;

        m_LightVerts = new Vector3[m_SubDivision];

        for (int i = 0; i < m_SubDivision; ++i)
        {
            float ratio = ((float)i) * n;            // 원을 SubDivison으로 나눈 후 몇번째의 원호인지
            float theta = (Mathf.PI * 2f) * ratio;   // 원주 * 비율 = 각도. ration를 radian으로 표시
            float x = Mathf.Cos(theta) * radius;
            float y = Mathf.Sin(theta) * radius;

            Debug.Log(Mathf.Cos(theta));

            origCircleVert = new Vector3(x, y, 1); //>> z축 중심으로 만든 원의 좌표

            // 회전행렬을 통해 origCircleVert의 각 점을 회전시킨다.
            Quaternion rotation = Quaternion.Euler(this.transform.eulerAngles.x,
                                                   this.transform.eulerAngles.y,
                                                   this.transform.eulerAngles.z);
            Matrix4x4 rotMatrix = Matrix4x4.Rotate(rotation);
            newCircleVert = rotMatrix.MultiplyPoint3x4(origCircleVert);

            // 회전한 newCircleVert의 위치를 향해 length 길이만큼 레이를 쏴서,
            // 부딪힌 점의 정보를 lightVertlist에 저장한다.
            Vector3 raycastPoint = SetRaycastPoint(newCircleVert, length);

            m_LightVerts[i] = raycastPoint;
        }
    }


    // lightVerts를 받아서 메쉬를 만들어준다.
    private void BuildConeMesh(Vector3[] _lightVerts)
    {
        if (_lightVerts.Length <= 1) return;

        // +1 해주는 이유 : 맨 처음 시작점 개수를 추가해줘야 한다.
        Vector3[] vertices = new Vector3[_lightVerts.Length + 1];

        // 삼각형 개수 : vertexCnt-1
        // 삼각형 꼭지점의 수 : 삼각형 개수 * 3
        int[] triangles = new int[_lightVerts.Length * 3];

        // 유니티 메시는 로컬좌표가 기준이다.
        vertices[0] = Vector3.zero;

        for (int i = 0; i < _lightVerts.Length; ++i)
        {
            // InverseTransformPoint : 로컬좌표 위치 받아오기.
            vertices[i + 1] = transform.InverseTransformPoint(_lightVerts[i]);

            // triangle 배열에 순서대로 대입
            if (i < _lightVerts.Length-1)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }
        // 시작점, 마지막점, 처음점을 마지막 삼각형으로 연결해준다.
        triangles[(_lightVerts.Length - 1) * 3] = 0;
        triangles[(_lightVerts.Length - 1) * 3 + 1] = _lightVerts.Length;
        triangles[(_lightVerts.Length - 1) * 3 + 2] = 1;
         

        m_LightMesh.Clear();
        m_LightMesh.vertices = vertices;
        m_LightMesh.triangles = triangles;
        m_LightMesh.RecalculateNormals();    // 노말 달아주기

        // 콜라이더는 aabb tree를 사용하기 때문에, 메쉬가 변경되면 다시 빌드해주어야 한다.
        m_MeshCollider.enabled = false;
        m_MeshCollider.enabled = true;
    }

    // 방향벡터와 길이를 받아와서 레이를 쏘고 point를 반환한다.
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

    // 각도를 수치로 입력받으면 내 현재 각도에서 그만큼 회전한 방향의 방향벡터를 반환한다. : 기준벡터
    private Vector3 DirFromAngle(float _angleDegree, float _verticalAngleDegree, bool _angleIsGlobal)
    {
        if (!_angleIsGlobal)
        {
            _angleDegree += transform.eulerAngles.y;
            _verticalAngleDegree += transform.eulerAngles.x;
        }


        // 20221108 양우석 : 해결했음.
        return new Vector3(Mathf.Cos((-_angleDegree + 90f) * Mathf.Deg2Rad),
                            Mathf.Tan(-_verticalAngleDegree * Mathf.Deg2Rad),
                            Mathf.Sin((-_angleDegree + 90f) * Mathf.Deg2Rad)).normalized;
    }

}
