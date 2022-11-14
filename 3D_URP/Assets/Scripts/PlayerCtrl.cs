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


    // SendMessage 타입으로 호출할 때 함수이름(콜백)
    // Void On액션명 (Input Asset에서 붙인 액션명)
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
        // 로비 접속할 때 설정한 닉네임을 텍스트에 표시
        txt_playerName.text = PhotonNetwork.NickName;


        // 내꺼면 초록색, 아니면 빨간색
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
