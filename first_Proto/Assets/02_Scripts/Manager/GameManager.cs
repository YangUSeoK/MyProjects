using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }
    private static GameManager instance;

    private EnemyManager enemyManager = null;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(this);
        }

        DontDestroyOnLoad(gameObject);

        //private EnemyManager = 

    }

    public bool mbIsGameOver = false;


    private void GameOver()
    {
        mbIsGameOver = true;

    }

}
