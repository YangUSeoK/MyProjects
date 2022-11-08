using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{
    public int playerDamage;
    private Animator animator;
    private Transform target;
    private bool skipMove;

    protected override void Start()
    {
        GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }


    protected override void AttemptMove<T> (int _xDir, int _yDir)
    {
        if (skipMove)
        {
            skipMove = false;
            return;
        }

        base.AttemptMove<T>(_xDir, _yDir);

        skipMove = true;
    }

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        // 적이 내 위치를 향해 움직인다.
        if(Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
        {
            yDir = target.position.y > transform.position.y ? 1 : -1;
        }
        else
        {
            xDir = target.position.x > transform.position.x ? 1 : -1;
        }

        AttemptMove<Player>(xDir, yDir);
    }

    protected override void OnCantMove<T> (T _component)
    {
        Player hitPlayer = _component as Player;

        animator.SetTrigger("enemyAttack");

        hitPlayer.LoseFood(playerDamage);
    }

}
