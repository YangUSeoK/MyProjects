using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;  // 시스템, 유니티시스템에 둘다 랜덤 있음

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int min;
        public int max;

        public Count(int _min, int _max)
        {
            this.min = _min;
            this.max = _max;
        }
    }

    public int columns = 8;
    public int rows = 8;
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();


    //private void Start()
    //{
    //    SetupScene(1);
    //}

    // 그리드포지션 리스트 초기화 후 다시 0~크기 까지 리스트 만들어줌
    // 2차원 좌표를 1차원으로 순서대로 쭉 리스트에 담는다.
    private void InitialiseList()
    {
        gridPositions.Clear();
        for (int x = 1; x < columns - 1; ++x)
        {
            for(int y = 1; y < rows -1; ++y)
            {
                gridPositions.Add(new Vector3(x,y,0f));
            }

        }
    }

    private void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        // 벽, 바닥 만들기
        for (int x = -1; x < columns + 1; ++x)
        {
            for(int y = -1; y < rows + 1; ++y)
            {
                // 바닥 쭉 깔고
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if(x == -1 || x == columns || y == -1 || y == rows)
                {
                    // 바깥 가장자리면 외벽을 깜
                   toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    // 랜덤포지션 위치 만들어줌 (겹치는거 생성방지)
    private Vector3 RandomPositon()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex); // 같은위치에 두개 생성 방지.
        
        return randomPosition;
    }

    // 실제로 오브젝트를 생성해줌 (무슨 오브젝트, 몇개부터, 몇개까지)
    private void LayoutObjectAtRandom(GameObject[] _tileArray, int _min, int _max)
    {
        // int니까 랜덤+1 해줘야함(이상~미만)
        int objectCount = Random.Range(_min, _max + 1);

        for(int i = 0; i < objectCount; ++i)
        {
            Vector3 randomPosition = RandomPositon();
            GameObject tileChoice = _tileArray[Random.Range(0, _tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    // 외부에서 불러온다. 스테이지를 만들어 줌
    public void SetupScene(int level)
    {
        BoardSetup();
        InitialiseList();
        LayoutObjectAtRandom(wallTiles, wallCount.min, wallCount.max);
        LayoutObjectAtRandom(foodTiles, foodCount.min, foodCount.max);
        int enemyCount = (int)Mathf.Log(level, 2f);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }
}
