using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private Renderer[] renderers = null;
    private Animator anim = null;
    private CharacterController cc = null;

    int initHp = 100;
    public int curHp;


    readonly int hashDie = Animator.StringToHash("Die");
    readonly int hashRespawn = Animator.StringToHash("Respawn");

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();

        curHp = initHp;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (curHp > 0 && collision.collider.CompareTag("BULLET"))
        {
            curHp -= 20;
            if (curHp <= 0)
            {
                // 플레이어 사망 코루틴
                StartCoroutine(PlayerDieCoroutine());
            }
        }
    }

    private IEnumerator PlayerDieCoroutine()
    {
        cc.enabled = false;
        anim.SetBool(hashRespawn, false);
        anim.SetTrigger(hashDie);

        yield return new WaitForSeconds(3f);

        anim.SetBool(hashRespawn, true);
        // 캐릭터 안보이게
        SetPlayerVisible(false);


        yield return new WaitForSeconds(1.5f);

        // 부활할 때 기존 스폰위치에서 랜덤생성
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);
        transform.position = points[idx].position;


        curHp = initHp;
        // 캐릭터 보이게
        SetPlayerVisible(true);

        cc.enabled = true;
    }

    // 플레이어 아래 구조가 너무 복잡해서 이렇게 처리해줌.
    private void SetPlayerVisible(bool isVisible)
    {
        foreach(var renderer in renderers)
        {
            renderer.enabled = isVisible;
        }
    }
}
