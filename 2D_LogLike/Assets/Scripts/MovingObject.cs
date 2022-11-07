using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime = 0;

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();

        // ���� ���ڸ� �����صּ� ���ϱ�� ȿ�������� �� �� �ִ�.
        inverseMoveTime = 1f / moveTime;
    }

    protected bool Move (int _xDir, int _yDir, out RaycastHit2D _hit)
    {
        Vector2 startPos = transform.position;
        Vector2 endPos = new Vector2(_xDir, _yDir);

        // ���� �� �� �ڱ����� �ε����� �ʵ���
        boxCollider.enabled = false;

        // ������ġ���� ��ǥ��ġ���� ���̸� ���
        _hit = Physics2D.Linecast(startPos, endPos, blockingLayer);
        boxCollider.enabled = true;

        if (_hit.transform == null){
            StartCoroutine(SmoothMovementCoroutine(endPos));
            return true;
        }

        return false;
    }


    // T�� ���� ���� : Player�� Enemy ��� MovingObject �� ��ӹ��� �ٵ�,
    // Player�� ��� Wall, Enemy�� ��� Player ���� �渷�� ���Ѵ�.
    // ��Ÿ�� ���� ��ȣ�ۿ��� hitComponent �� �� �� ����.
    // �Ϲ����� ����ؼ� �̰� ���� �����س��� ��ӹ޴� �ֵ����׼� ������ �������ؼ� ��� ����
    protected virtual void AttemptMove <T> (int _xDir, int _yDir) where T : Component
    {
        RaycastHit2D hit;
        
        // �������� �����ϸ� true, �ƴϸ� false ��ȯ.
        bool canMove = Move(_xDir, _yDir, out hit);

        // hit�� ���۷�����(out) ���� ������ ���� ���Ϲ޴´�.
        // => �ƹ��͵� �� �ε������� �׳� �������� �� => �ٷ� return
        if(hit.transform == null)
        {
            return;
        }
        
        // �ε������� ������Ʈ�� hitComponent ������ �޾ƿ´�.
        T hitComponent = hit.transform.GetComponent<T>();

        // ������ �� ����, ���̿� �������� �ִٸ� => �渷�ϴ� ���� �ִ�
        if(!canMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
        }

    }

    protected IEnumerator SmoothMovementCoroutine(Vector3 _endPos)
    {
        float sqrRemainingDistance = (transform.position - _endPos).sqrMagnitude;

        while(sqrRemainingDistance > float.Epsilon) // 0�� ����� ���� ���� ���ڿ� ��
        {
            // Vector3.MoveTowards : ���� ����Ʈ�� �������� ��ǥ ����Ʈ�� ��ġ��Ų��.
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, _endPos, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - _endPos).sqrMagnitude;

            yield return null;
        }
    }

    // �Ϲ��� �Է� T�� Component ��� ������ �޾ƿ´�.
    protected abstract void OnCantMove <T> (T component) where T : Component;

}

