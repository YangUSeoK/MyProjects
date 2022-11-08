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
        // 플레이어 턴이 아니면 리턴
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
            // 플레이어가 <벽>과 부딪힐지 확인하고, 가능하면 Move 실행
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

    // T : 마주치는 대상의 컴포넌트(호출할 때 정해준다)
    protected override void AttemptMove<T>(int _xDir, int _yDir)
    {
        // 한칸 움직일 때 마다 음식 1 깎임
        food--;

        // 부딪힌 컴포넌트 정보를 가지고 부모의 AttemptMove를 호출한다.
        base.AttemptMove<T>(_xDir, _yDir);

        RaycastHit2D hit;
        CheckIfGameOver();

        GameManager.instance.playersTurn = false;
    }

    // c 소문자임
    protected override void OnCantMove<T>(T _component)
    {
        // 입력받은 component 를 Wall 로 변환해서 (as Wall) hitWall 에 대입한다.
        Wall hitWall = _component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playerChop");
    }

    private void Restart()
    {
        // 마지막으로 로드 된 씬을 로드한다. = main씬
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
