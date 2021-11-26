using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8;
    public int rows = 8;
    public Count wallCount = new Count(5, 9);

    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] outerWallTilesTiles;
    public GameObject[] enemyTiles;
    public GameObject[] itemTiles;

    public GameObject player;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    #region Random grid board
    void InitializeGrid()
    {
        gridPositions.Clear();

        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void RandomBoardSetup(int enemyCount)
    {
        boardHolder = new GameObject("Board").transform;
        
        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || x == columns || y == -1 || y == rows)
                    toInstantiate = outerWallTilesTiles[Random.Range(0, outerWallTilesTiles.Length)];

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity);

                instance.transform.SetParent(boardHolder);
            }
        }

        InitializeGrid();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(player, new Vector3(1, 1, 0f), Quaternion.identity);
    }

    /// <summary>
    /// Returns a random position from the grid list AND REMOVES that position from the grid list.
    /// </summary>
    /// <returns>A position.</returns>
    Vector3 RandomGridPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1); 

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomGridPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    #endregion

    void SpawnItemReward()
    {
        GameObject item = itemTiles[Random.Range(0, itemTiles.Length)];
        item = Instantiate(item, RandomGridPosition(), Quaternion.identity) as GameObject;
    }

    void SpawnExit()
    {
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }

    public void FinishLevel()
    {
        SpawnItemReward();
        SpawnExit();
    }

    public void SetupScene(int level)
    {
        RandomBoardSetup(level);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
