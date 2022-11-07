using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private GameObject[] mEnemys = null;


    private void Awake()
    {
        mEnemys = GetComponentsInChildren<GameObject>();
    }

    public void GameOver()
    {
        foreach(GameObject enemy in mEnemys)
        {
            enemy.GetComponent<EnemyMove>().Agent.isStopped = true;
        }
    }
}
