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
    }

    public override void Action()
    {
        // ���� ���̴��� üũ -> ���� ���̸� �� ��ġ�� ����
        // ������ ��ġ�� �̵�

        if(m_FOVForLight.IsInFOV() && m_FOVForLight.IsLookDirect())
        {

        }

    }

    public override void CheckState()
    {
        // ������ ��ġ�� ��� ���̸� ���.
        
        // if(���θ��°� ���� �Ÿ��� �������� ���̶��)


        // ������ �� ��ġ�� ���� ��ġ�� ����(�ԽǷ� ���) ���̰� �ȸ�����

    }

    

    public override void ExitState()
    {
    }
}
