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
                // �÷��̾� ��� �ڷ�ƾ
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
        // ĳ���� �Ⱥ��̰�
        SetPlayerVisible(false);


        yield return new WaitForSeconds(1.5f);

        // ��Ȱ�� �� ���� ������ġ���� ��������
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);
        transform.position = points[idx].position;


        curHp = initHp;
        // ĳ���� ���̰�
        SetPlayerVisible(true);

        cc.enabled = true;
    }

    // �÷��̾� �Ʒ� ������ �ʹ� �����ؼ� �̷��� ó������.
    private void SetPlayerVisible(bool isVisible)
    {
        foreach(var renderer in renderers)
        {
            renderer.enabled = isVisible;
        }
    }
}
