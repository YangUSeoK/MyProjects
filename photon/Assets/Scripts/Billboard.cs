using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform canvas;

    private void Update()
    {
        // ĵ���� ���� = ī�޶� ���� ���� �����༭ ȸ���ص� ǥ�õ�
        canvas.forward = Camera.main.transform.forward;
    }

}
