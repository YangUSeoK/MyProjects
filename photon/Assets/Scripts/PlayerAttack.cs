using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerAttack : MonoBehaviourPun
{
    public Animator anim = null;

    public Slider hpSlider = null;
    public BoxCollider weaponCol = null;
    public float attackPower = 2f;
    public float maxHp = 10f;
    private float curHp = 0f;

    private void Start()
    {
        curHp = maxHp;
        hpSlider.value = curHp / maxHp;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (photonView.IsMine)
            {
                // AttackAnimation 함수를 RPC로 호출. 호출 대상은 서버에 있는 모든 P1 객체(본체, 클론 포함)에서 실행.
                photonView.RPC("AttackAnimation", RpcTarget.AllBuffered);
            }
        }
    }


    // RPC를 쓰려면 [PunRPC] 를 추가해줘야 함
    [PunRPC]
    public void AttackAnimation()
    {
        anim.SetTrigger("Attack");
    }

    [PunRPC]
    public void Damaged(float _power) 
    {
        // 이런식으로 체력관리할 수도 있음.
        curHp = Mathf.Max(0, curHp - _power);
        hpSlider.value = curHp / _power;
    }

    // 내가 상대방을 때릴 때 
    private void OnTriggerEnter(Collider other)
    {
        // 내가 조종하는 캐릭터가 PLAYER와 부딪혔다면
        if (photonView.IsMine && other.gameObject.CompareTag("PLAYER"))
        {
            // 공격당한 대상의 포톤뷰에서 RPC를 호출한다.
            PhotonView pv = other.gameObject.GetComponent<PhotonView>();
            pv.RPC("Damaged", RpcTarget.AllBuffered, attackPower);

            // 공격 이후 무기 콜라이더 비활성화. 최초로 맞은 한놈만 때린다.
            weaponCol.enabled = false;
        }
    }


}
