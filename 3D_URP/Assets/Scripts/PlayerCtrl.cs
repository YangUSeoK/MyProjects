using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrl : MonoBehaviour
{

    // SendMessage Ÿ������ ȣ���� �� �Լ��̸�(�ݹ�)
    // Void On�׼Ǹ� (Input Asset���� ���� �׼Ǹ�)
    private void OnMove(InputValue value)
    {
        Vector2 dir = value.Get<Vector2>();
        print($"Move = {dir.x} / {dir.y}");
        moveDir = new Vector3(dir.x, 0, dir.y);

        anim.SetFloat("Movement", dir.magnitude);
    }

    private void OnAttack()
    {
        print("Attack");
        anim.SetTrigger("Attack");
    }

    Animator anim = null;
    Vector3 moveDir = Vector3.zero;
    private float moveSpeed = 4f;


    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if(moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDir);
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime );
        }
    }
}
