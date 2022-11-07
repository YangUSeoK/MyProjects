using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraRot : MonoBehaviour
{
    [SerializeField] private Transform m_PlayerTr = null;
    
    
    private float mAxisX = 0f;

    private float mRotSpeed = 100f;
    private float mXRotate = 0f;

    private void Awake()
    {
        transform.eulerAngles = Vector3.zero;
    }

    private void Update()
    {
        // 화면 회전
        // 마우스는 둘이 바뀌어야 함
        mAxisX = -Input.GetAxis("Mouse Y");

        mXRotate = Mathf.Clamp(mXRotate + (mAxisX * Time.deltaTime * mRotSpeed), -90f, 90f);

        transform.position = m_PlayerTr.position + Vector3.up + (m_PlayerTr.transform.forward * 0.5f);
        transform.eulerAngles = new Vector3(mXRotate, m_PlayerTr.eulerAngles.y, 0f);
    }
}
