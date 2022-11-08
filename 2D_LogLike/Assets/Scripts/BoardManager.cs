using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;  // �ý���, ����Ƽ�ý��ۿ� �Ѵ� ���� ����

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

    // �׸��������� ����Ʈ �ʱ�ȭ �� �ٽ� 0~ũ�� ���� ����Ʈ �������
    // 2���� ��ǥ�� 1�������� ������� �� ����Ʈ�� ��´�.
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

        // ��, �ٴ� �����
        for (int x = -1; x < columns + 1; ++x)
        {
            for(int y = -1; y < rows + 1; ++y)
            {
                // �ٴ� �� ���
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if(x == -1 || x == columns || y == -1 || y == rows)
                {
                    // �ٱ� �����ڸ��� �ܺ��� ��
                   toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    // ���������� ��ġ ������� (��ġ�°� ��������)
    private Vector3 RandomPositon()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex); // ������ġ�� �ΰ� ���� ����.
        
        return randomPosition;
    }

    // ������ ������Ʈ�� �������� (���� ������Ʈ, �����, �����)
    private void LayoutObjectAtRandom(GameObject[] _tileArray, int _min, int _max)
    {
        // int�ϱ� ����+1 �������(�̻�~�̸�)
        int objectCount = Random.Range(_min, _max + 1);

        for(int i = 0; i < objectCount; ++i)
        {
            Vector3 randomPosition = RandomPositon();
            GameObject tileChoice = _tileArray[Random.Range(0, _tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    // �ܺο��� �ҷ��´�. ���������� ����� ��
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
