using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private MeshCollider m_MeshCollider = null;


    // ray의 정보를 저장하는 구조체
    private struct RaycastInfo
    {
        public bool isHit;
        public Vector3 pointPos;
        public float distance;
        public float angle;

        // 생성자
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

        // 프로퍼티에 계속 접근하는거 싫어서 변수에 저장.
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
        // 각도 * 비율 반올림
        int stepCnt = Mathf.RoundToInt(m_Light.spotAngle * m_MeshRate);

        // 다시 빛의 각도로 나눠서 쪼개지는 앵글의 각을 구함
        float stepAngleSize = m_Light.spotAngle / stepCnt;

        List<Vector3> lightPointList = new List<Vector3>();

        // 부채꼴로 레이를 쏴서 구자체에 정보를 저장한다.
        for (int i = 0; i <= stepCnt; ++i)
        {
            float angle = transform.eulerAngles.y - (m_Light.spotAngle / 2) + (stepAngleSize * i);

            // 디버그용 DrawLine
            //Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle, true) * m_LightRange, Color.green);

            RaycastInfo newRayCastInfo = SetRaycastInfo(angle);
            lightPointList.Add(newRayCastInfo.pointPos);
        }


        // 아래부터는 메쉬 만드는 부분


        // 20221106 양우석 : 메쉬 만드는 함수는 따로 쪼갤 것
        // +1 해주는 이유 : 맨 처음 시작점 개수를 추가해줘야 한다.
        int vertexCnt = lightPointList.Count + 1;
        Vector3[] vertices = new Vector3[vertexCnt];



        // 삼각형 개수 : vertexCnt-2
        // 삼각형 꼭지점의 수 : 삼각형 개수 * 3
        int[] triangles = new int[(vertexCnt - 2) * 3];

        // 유니티 메시는 로컬좌표가 기준이다.
        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexCnt - 1; ++i)
        {
            // InverseTransformPoint : 로컬좌표 위치 받아오기.
            vertices[i + 1] = transform.InverseTransformPoint(lightPointList[i]);

            // 배열맵핑으로 수정
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
        

        // 콜라이더는 aabb tree를 사용하기 때문에, 메쉬가 변경되면 다시 빌드해주어야 한다.
        m_MeshCollider.enabled = false;
        m_MeshCollider.enabled = true;

        
    }


    // tarnsform의 Euler.y 값을 인자로 받아와서, 그 방향으로 Ray를 쏴서 hit 정보를 구조체에 저장한다.
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
