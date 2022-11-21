using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV : MonoBehaviour
{
    private bool mbIsTurnOn = true; // 20221118 양우석 : 나중에 cctv매니저 만들면 false로 해야함. 
    public bool IsTurnOn
    {
        set { mbIsTurnOn = value; }
    }
    private bool mbIsDetect = false;
    private Vector3 m_OriAngle;
    private Transform m_TargetTr;

    // Ray 관련
    private Light m_Light = null;
    [SerializeField] private float m_RayToPlayerDistance = 100f;
    private int m_LayerMask = 0;
    // 레이를 쪼개는 비율. 클수록 잘게 쪼갠다
    [SerializeField] private int m_SubDivision = 10;


    [SerializeField] private float m_WarningTimer = 3f;
    

    private void Awake()
    {
        m_Light = GetComponent<Light>();
    }

    private void Start()
    {
        m_OriAngle = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        m_LayerMask = 1 << LayerMask.NameToLayer("PLAYER") | 1 << LayerMask.NameToLayer("OBSTACLE");
    }

    private void LateUpdate()
    {
        if (mbIsTurnOn)
        {
            RayCone();
        }
    }

    // 플레이어 발견 -> 플레이어 바라봄 -> 플레이어위치로 레이 쏨 -> 장애물 뒤에 안숨으면 x초 후 경보발령
    private IEnumerator DetectPlayerCoroutine(Vector3 _playerPos)
    {
        transform.LookAt(_playerPos);
        WaitForSeconds ws01 = new WaitForSeconds(0.01f);
        float timer = 0;

        while (mbIsDetect)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, _playerPos - transform.position, out hitInfo, m_RayToPlayerDistance, m_LayerMask))
            {
                if (hitInfo.transform.CompareTag("PLAYER"))
                {
                    m_TargetTr = hitInfo.transform;
                    // 시간초 세기
                    timer += 0.01f;
                    Debug.Log(timer);
                    if (timer >= m_WarningTimer)
                    {
                        Debug.Log("CCTV : 경보 발령!");
                        // 경보발령
                        // 20221118 양우석 : CCTV 매니저한테 델리게이트로 플레이어 위치를 전해주면 됨.
                        // CCTV매니저 -> Enemy매니저 -> 플레이어 위치 일정 거리 내 좀비들 상태 TracePlayer로 바꿔주고 _playerPos 전달

                        timer = 0f; // 경보후에 타이머 초기화
                    }
                }
                else
                {
                    SetUnDetect();
                }
            }
            else
            {
                SetUnDetect();
            }

            Debug.Log("CCTV : 플레이어 위치 추적.");
            transform.LookAt(m_TargetTr.position);
            yield return ws01;
        }
    }

    private void SetUnDetect()
    {
        Debug.Log("CCTV : 플레이어를 놓쳤다.");
        mbIsDetect = false;
        m_TargetTr = null;
    }

    private void RayCone()
    {
        float n = 1f / (m_SubDivision);
        float radius = Mathf.Tan((m_Light.spotAngle * 0.5f) * Mathf.Deg2Rad);
        float length = m_Light.range / Mathf.Cos(m_Light.spotAngle * 0.5f * Mathf.Deg2Rad);
        Vector3 origCircleVert;
        Vector3 newCircleVert;

        for (int i = 0; i < m_SubDivision; ++i)
        {
            float ratio = ((float)i) * n;            // 원을 비율로 나눔
            float theta = (Mathf.PI * 2f) * ratio;   // 원주 * 비율 = 각도
            float x = Mathf.Cos(theta) * radius;
            float y = Mathf.Sin(theta) * radius;

            origCircleVert = new Vector3(x, y, 1); //>> z축 중심으로 만든 원의 좌표

            // 회전행렬을 통해 회전시킨다.
            Quaternion rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
            Matrix4x4 rotMatrix = Matrix4x4.Rotate(rotation);

            newCircleVert = rotMatrix.MultiplyPoint3x4(origCircleVert);

            //Debug.DrawLine(transform.position, newCircleVert, Color.green);

            // 이제 각각의 점 위치를 향해 length 길이만큼 레이를 쏘면 된다. 
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, newCircleVert, out hitInfo, m_Light.range, m_LayerMask))
            {
                if (hitInfo.transform.CompareTag("PLAYER"))
                {
                    Debug.Log("CCTV : 플레이어 발견!");
                    mbIsDetect = true;
                    m_TargetTr = hitInfo.transform;
                    StartCoroutine(DetectPlayerCoroutine(hitInfo.transform.position));
                }
            }
        }
    }
}
