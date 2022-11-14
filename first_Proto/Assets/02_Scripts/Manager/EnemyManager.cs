using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Enemy[] mEnemys = null;


    private void Awake()
    {
        mEnemys = GetComponentsInChildren<Enemy>();
    }

    public void GameOver()
    {
        foreach(Enemy enemy in mEnemys)
        {
            enemy.GetComponent<EnemyMove>().Agent.isStopped = true;
        }
    }
}
