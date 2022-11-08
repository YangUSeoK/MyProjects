using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingObject
{
    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;

    public float restartLevelDelay = 1f;

    private Animator animator;
    private int food;

    protected override void Start()
    {
        animator = GetComponent<Animator>();

        food = GameManager.instance.playerFoodPoints;

        base.Start();
    }

    private void Update()
    {
        // �÷��̾� ���� �ƴϸ� ����
        if (!GameManager.instance.playersTurn)
        {
            return;
        }

        int horizontal = 0;
        int vertical = 0;
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
        {
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0)
        {
            // �÷��̾ <��>�� �ε����� Ȯ���ϰ�, �����ϸ� Move ����
            AttemptMove<Wall>(horizontal, vertical);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Exit"))
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (collision.CompareTag("Food"))
        {
            food += pointsPerFood;
            collision.gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Soda"))
        {
            food += pointsPerSoda;
            collision.gameObject.SetActive(false);
        }
    }



    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }

    // T : ����ġ�� ����� ������Ʈ(ȣ���� �� �����ش�)
    protected override void AttemptMove<T>(int _xDir, int _yDir)
    {
        // ��ĭ ������ �� ���� ���� 1 ����
        food--;

        // �ε��� ������Ʈ ������ ������ �θ��� AttemptMove�� ȣ���Ѵ�.
        base.AttemptMove<T>(_xDir, _yDir);

        RaycastHit2D hit;
        CheckIfGameOver();

        GameManager.instance.playersTurn = false;
    }

    // c �ҹ�����
    protected override void OnCantMove<T>(T _component)
    {
        // �Է¹��� component �� Wall �� ��ȯ�ؼ� (as Wall) hitWall �� �����Ѵ�.
        Wall hitWall = _component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playerChop");
    }

    private void Restart()
    {
        // ���������� �ε� �� ���� �ε��Ѵ�. = main��
        Application.LoadLevel(Application.loadedLevel);
    }

    public void LoseFood(int _loss)
    {
        animator.SetTrigger("playerHit");
        food -= _loss;
        CheckIfGameOver();
    }
    private void CheckIfGameOver()
    {
        if (food <= 0)
        {
            GameManager.instance.GameOver();
        }
    }



}
