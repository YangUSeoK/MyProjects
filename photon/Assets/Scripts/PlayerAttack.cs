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
                // AttackAnimation �Լ��� RPC�� ȣ��. ȣ�� ����� ������ �ִ� ��� P1 ��ü(��ü, Ŭ�� ����)���� ����.
                photonView.RPC("AttackAnimation", RpcTarget.AllBuffered);
            }
        }
    }


    // RPC�� ������ [PunRPC] �� �߰������ ��
    [PunRPC]
    public void AttackAnimation()
    {
        anim.SetTrigger("Attack");
    }

    [PunRPC]
    public void Damaged(float _power) 
    {
        // �̷������� ü�°����� ���� ����.
        curHp = Mathf.Max(0, curHp - _power);
        hpSlider.value = curHp / _power;
    }

    // ���� ������ ���� �� 
    private void OnTriggerEnter(Collider other)
    {
        // ���� �����ϴ� ĳ���Ͱ� PLAYER�� �ε����ٸ�
        if (photonView.IsMine && other.gameObject.CompareTag("PLAYER"))
        {
            // ���ݴ��� ����� ����信�� RPC�� ȣ���Ѵ�.
            PhotonView pv = other.gameObject.GetComponent<PhotonView>();
            pv.RPC("Damaged", RpcTarget.AllBuffered, attackPower);

            // ���� ���� ���� �ݶ��̴� ��Ȱ��ȭ. ���ʷ� ���� �ѳ� ������.
            weaponCol.enabled = false;
        }
    }


}
