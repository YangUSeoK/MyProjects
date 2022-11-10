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

    // 람다식으로 이벤트 매핑
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
            // 총알 발사 (로컬 실행)
            FireBullet();

            // RPC 원격실행(함수명, 호출 대상, 전달 데이터)
            pv.RPC("FireBullet", RpcTarget.Others, null);

            // 호출대상이 RpcTarget.All 이라면 로컬꺼도 실행되기 때문에, 로컬실행구문이 필요없다.
        }
    }

    // 다른 컴퓨터에 있는 내 클론이 FireBullet을 실행한다.
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
