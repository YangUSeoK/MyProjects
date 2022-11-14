using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviourPunCallbacks
{
    private float mBlinkDistance = 3f;
    public Text txt_playerName = null;


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

    private void OnEnable()
    {
        // �κ� ������ �� ������ �г����� �ؽ�Ʈ�� ǥ��
        txt_playerName.text = PhotonNetwork.NickName;


        // ������ �ʷϻ�, �ƴϸ� ������
        if (photonView.IsMine)
        {
            txt_playerName.color = Color.green;
        }
        else
        {
            txt_playerName.color = Color.red;
        }
    }
    private void Update()
    {
        if(moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDir);
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime );
        }

        if (photonView.IsMine && Input.GetKeyDown(KeyCode.G))
        {
            Blink();
            //photonView.RPC("Blink", RpcTarget.AllBuffered, null);
        }

        Debug.Log(PhotonNetwork.NickName);
        Debug.Log(txt_playerName.text);
    }

    //[PunRPC]
    private void Blink()
    {
        CharacterController cc = GetComponent<CharacterController>();
        cc.enabled = false;

        transform.position = transform.position + transform.forward * mBlinkDistance;
        cc.enabled = true;  
    }

}
