using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BoardManager boardScript;

    public static GameManager instance = null;

    private int level = 1;
    private bool finished;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
        finished = false;
        InitGame();
    }

    void InitGame()
    {
        boardScript.SetupScene(level);
    }

    void OnLevelWasLoaded(int index)
    {
        finished = false;
        level++;
        InitGame();
    }

    
    // Update is called once per frame
    void Update()
    {
        GameObject[] enemiesPresent = GameObject.FindGameObjectsWithTag("Enemy");
        if (!finished && enemiesPresent.Length == 0)
        {
            boardScript.FinishLevel();
            finished = true;
        }
            
    }
}
