using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceLight_Slaughter : EnemyState
{
    public TraceLight_Slaughter(Enemy _enemy) : base("TraceLight", _enemy) { }

    private Vector3 m_LightPos;
    private Vector3 m_FlashPos;


public override void EnterState()
    {
        Debug.Log("TraceLight ����!");
    }

    public override void Action()
    {
        // ���� ���̴��� üũ -> ���� ���̸� �� ��ġ�� ����
        // ������ ��ġ�� �̵�
        Debug.Log("TraceLight �׼�!");

    }

    public override void CheckState()
    {
        Debug.Log("TraceLight üũüũ!");
        if (Vector3.Distance(m_LightPos, m_Enemy.transform.position)  <= 0.5f)
        {

        }

        // ������ ��ġ�� ��� ���̸� ���.
        
        // if(���θ��°� ���� �Ÿ��� �������� ���̶��)


        // ������ �� ��ġ�� ���� ��ġ�� ����(�ԽǷ� ���) ���̰� �ȸ�����

    }

    

    public override void ExitState()
    {
        Debug.Log("TraceLight ����!");
    }
}
