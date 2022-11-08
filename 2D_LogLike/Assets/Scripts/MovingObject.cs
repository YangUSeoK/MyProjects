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

        // 나눈 숫자를 저장해둬서 곱하기로 효율적으로 쓸 수 있다.
        inverseMoveTime = 1f / moveTime;
    }

    protected bool Move (int _xDir, int _yDir, out RaycastHit2D _hit)
    {
        Vector2 startPos = transform.position;
        Vector2 endPos = new Vector2(_xDir, _yDir);

        // 레이 쏠 때 자기한테 부딪히지 않도록
        boxCollider.enabled = false;

        // 시작위치부터 목표위치까지 레이를 쏜다
        _hit = Physics2D.Linecast(startPos, endPos, blockingLayer);
        boxCollider.enabled = true;

        if (_hit.transform == null){
            StartCoroutine(SmoothMovementCoroutine(endPos));
            return true;
        }

        return false;
    }


    // T를 쓰는 이유 : Player와 Enemy 모두 MovingObject 를 상속받을 텐데,
    // Player의 경우 Wall, Enemy의 경우 Player 에게 길막을 당한다.
    // 런타임 까지 상호작용할 hitComponent 를 알 수 없다.
    // 일반형을 사용해서 이걸 먼저 구현해놓고 상속받는 애들한테서 각각을 재정의해서 사용 가능
    protected virtual void AttemptMove <T> (int _xDir, int _yDir) where T : Component
    {
        RaycastHit2D hit;
        
        // 움직임이 가능하면 true, 아니면 false 반환.
        bool canMove = Move(_xDir, _yDir, out hit);

        // hit은 레퍼런스로(out) 들어갔기 때문에 값을 리턴받는다.
        // => 아무것도 안 부딪혔으면 그냥 움직여도 됨 => 바로 return
        if(hit.transform == null)
        {
            return;
        }
        
        // 부딪힌놈의 컴포넌트를 hitComponent 변수로 받아온다.
        T hitComponent = hit.transform.GetComponent<T>();

        // 움직일 수 없고, 레이에 맞은놈이 있다면 => 길막하는 놈이 있다
        if(!canMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
        }

    }

    protected IEnumerator SmoothMovementCoroutine(Vector3 _endPos)
    {
        float sqrRemainingDistance = (transform.position - _endPos).sqrMagnitude;

        while(sqrRemainingDistance > float.Epsilon) // 0에 가까운 아주 작은 숫자와 비교
        {
            // Vector3.MoveTowards : 현재 포인트를 직선상의 폭표 포인트로 위치시킨다.
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, _endPos, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - _endPos).sqrMagnitude;

            yield return null;
        }
    }

    // 일반형 입력 T를 Component 라는 변수로 받아온다.
    protected abstract void OnCantMove <T> (T component) where T : Component;

}

