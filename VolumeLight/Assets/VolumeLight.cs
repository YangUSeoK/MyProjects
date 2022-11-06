using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeLight : MonoBehaviour
{

    // Ray 관련
    private Light m_Light = null;
    private float m_LightRange = 0f;
    
    // 레이를 쪼개는 비율. 클수록 잘게 쪼갠다
    [SerializeField] private float m_MeshRate = 0.1f;


    // Mesh 생성관련. 20221106 양우석 : 나중에 클래스 쪼갤 수도 있음.
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


    // ray의 정보를 저장하는 구조체
    public struct RaycastInfo
    {
        public bool hit;
        public Vector3 pointPos;
        public float range;
        public float angle;

        // 생성자
        public RaycastInfo(bool _hit, Vector3 _pointPos, float _range, float _angle)
        {
            hit = _hit;
            pointPos = _pointPos;
            range = _range;
            angle = _angle;
        }
    }

    // tarnsform의 Euler.y 값을 인자로 받아와서, 그 방향으로 Ray를 쏴서 hit 정보를 구조체에 저장한다.
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

    // 각도를 수치로 입력받으면 내 현재 각도에서 그만큼 회전한 방향의 방향벡터를 반환한다.
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
        // 각도 * 비율 반올림
        int stepCnt = Mathf.RoundToInt(m_Light.spotAngle * m_MeshRate);

        // 다시 빛의 각도로 나눠서 쪼개지는 앵글의 각을 구함
        float stepAngleSize = m_Light.spotAngle / stepCnt;

        List<Vector3> lightPointList = new List<Vector3>();

        for(int i = 0; i <= stepCnt; ++i)
        {
            float angle = transform.eulerAngles.y - (m_Light.spotAngle / 2) + (stepAngleSize * i);

            // 디버그용 DrawLine
            //Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle, true) * m_Light.spotAngle, Color.green);

            RaycastInfo newRayCastInfo = SetRaycastInfo(angle);
            lightPointList.Add(newRayCastInfo.pointPos);
        }

        // 20221106 양우석 : 메쉬 만드는 함수는 따로 쪼갤 것
        // +1 해주는 이유 : 맨 처음 시작점 개수를 추가해줘야 한다.
        int vertexCnt = lightPointList.Count + 1;
        Vector3[] vertices = new Vector3[vertexCnt];

        // 20221106 양우석 : 이부분 배열맵핑으로 수정할 수 있음. 내일 할 것
        int[] triangles = new int[(vertexCnt - 2) * 3]; // 삼각형 개수 vertexCnt-2  꼭지점 개수 : 삼각형개수 *3

        // 유니티 메시는 로컬좌표가 기준이다.
        vertices[0] = Vector3.zero;

        for(int i = 0; i < vertexCnt-1; ++i)
        {
            vertices[i+1] = transform.InverseTransformPoint(lightPointList[i]);

            // 배열맵핑으로 수정
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
