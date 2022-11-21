using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV : MonoBehaviour
{
    private bool mbIsTurnOn = true; // 20221118 ��켮 : ���߿� cctv�Ŵ��� ����� false�� �ؾ���. 
    public bool IsTurnOn
    {
        set { mbIsTurnOn = value; }
    }
    private bool mbIsDetect = false;
    private Vector3 m_OriAngle;
    private Transform m_TargetTr;

    // Ray ����
    private Light m_Light = null;
    [SerializeField] private float m_RayToPlayerDistance = 100f;
    private int m_LayerMask = 0;
    // ���̸� �ɰ��� ����. Ŭ���� �߰� �ɰ���
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

    // �÷��̾� �߰� -> �÷��̾� �ٶ� -> �÷��̾���ġ�� ���� �� -> ��ֹ� �ڿ� �ȼ����� x�� �� �溸�߷�
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
                    // �ð��� ����
                    timer += 0.01f;
                    Debug.Log(timer);
                    if (timer >= m_WarningTimer)
                    {
                        Debug.Log("CCTV : �溸 �߷�!");
                        // �溸�߷�
                        // 20221118 ��켮 : CCTV �Ŵ������� ��������Ʈ�� �÷��̾� ��ġ�� �����ָ� ��.
                        // CCTV�Ŵ��� -> Enemy�Ŵ��� -> �÷��̾� ��ġ ���� �Ÿ� �� ����� ���� TracePlayer�� �ٲ��ְ� _playerPos ����

                        timer = 0f; // �溸�Ŀ� Ÿ�̸� �ʱ�ȭ
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

            Debug.Log("CCTV : �÷��̾� ��ġ ����.");
            transform.LookAt(m_TargetTr.position);
            yield return ws01;
        }
    }

    private void SetUnDetect()
    {
        Debug.Log("CCTV : �÷��̾ ���ƴ�.");
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
            float ratio = ((float)i) * n;            // ���� ������ ����
            float theta = (Mathf.PI * 2f) * ratio;   // ���� * ���� = ����
            float x = Mathf.Cos(theta) * radius;
            float y = Mathf.Sin(theta) * radius;

            origCircleVert = new Vector3(x, y, 1); //>> z�� �߽����� ���� ���� ��ǥ

            // ȸ������� ���� ȸ����Ų��.
            Quaternion rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
            Matrix4x4 rotMatrix = Matrix4x4.Rotate(rotation);

            newCircleVert = rotMatrix.MultiplyPoint3x4(origCircleVert);

            //Debug.DrawLine(transform.position, newCircleVert, Color.green);

            // ���� ������ �� ��ġ�� ���� length ���̸�ŭ ���̸� ��� �ȴ�. 
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, newCircleVert, out hitInfo, m_Light.range, m_LayerMask))
            {
                if (hitInfo.transform.CompareTag("PLAYER"))
                {
                    Debug.Log("CCTV : �÷��̾� �߰�!");
                    mbIsDetect = true;
                    m_TargetTr = hitInfo.transform;
                    StartCoroutine(DetectPlayerCoroutine(hitInfo.transform.position));
                }
            }
        }
    }
}
