using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform canvas;

    private void Update()
    {
        // 캔버스 정면 = 카메라 정면 으로 맞춰줘서 회전해도 표시됨
        canvas.forward = Camera.main.transform.forward;
    }

}
