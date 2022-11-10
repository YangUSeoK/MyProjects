using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Fire : MonoBehaviour
{
    public Transform firePos;
    public GameObject bulletPrefab;

    private ParticleSystem muzzleFlash = null;
    PhotonView pv = null;

    // ���ٽ����� �̺�Ʈ ����
    bool isMouseLClick => Input.GetMouseButtonDown(0);

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        muzzleFlash = firePos.Find("MuzzleFlash").GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if(pv.IsMine && isMouseLClick)
        {
            // �Ѿ� �߻� (���� ����)
            FireBullet();

            // RPC ���ݽ���(�Լ���, ȣ�� ���, ���� ������)
            pv.RPC("FireBullet", RpcTarget.Others, null);

            // ȣ������ RpcTarget.All �̶�� ���ò��� ����Ǳ� ������, ���ý��౸���� �ʿ����.
        }
    }

    // �ٸ� ��ǻ�Ϳ� �ִ� �� Ŭ���� FireBullet�� �����Ѵ�.
    [PunRPC]
    private void FireBullet()
    {
        if (!muzzleFlash.isPlaying)
        {
            muzzleFlash.Play();
        }

        GameObject bullet = Instantiate(bulletPrefab, firePos.position, firePos.rotation);
    }
}
