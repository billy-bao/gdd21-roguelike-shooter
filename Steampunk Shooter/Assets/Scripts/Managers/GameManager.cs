using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;

    public GameObject defaultLevelManager;
    private LevelManager curLevelManager;

    public GameObject playerObj;
    private Player player;

    private Map map;
    private int curLevelId = -1;
    private int enteringDir = -1;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        player = playerObj.GetComponent<Player>();
        DontDestroyOnLoad(playerObj);
        DontDestroyOnLoad(FindObjectOfType<HealthBar>().transform.parent.gameObject);
        //SceneManager.sceneLoaded += OnLevelLoaded;

        map = GetComponent<MapGenerator>().GenerateMap(20);
        LoadLevel(map.startId, -1);
    }

    void LoadLevel(int id, int dir)
    {
        curLevelId = id;
        enteringDir = dir;
        SceneManager.LoadScene(map.levels[id].sceneName);
    }

    void OnLevelWasLoaded(int a)//Scene scene, LoadSceneMode mode)
    {
        // find level manager & data
        GameObject obj = GameObject.FindGameObjectWithTag("LevelData");
        if (obj == null)
        {
            Debug.Log("No level data found! Aborting...");
            return;
        }
        else
        {
            curLevelManager = obj.GetComponent<LevelManager>();
            if(curLevelManager == null)
            {
                Debug.Log("Using default LevelManager.");
                curLevelManager = Instantiate(defaultLevelManager).GetComponent<LevelManager>();
                curLevelManager.levelData = obj.GetComponent<LevelData>();
            }
        }
        curLevelManager.Initialize(map.flags[curLevelId], player, enteringDir);
        curLevelManager.gameManager = this;
    }

    public void ExitLevel(int dir)
    {
        Map.Coords newCoords = Map.MoveDir(map.levels[curLevelId].coords, dir);
        LevelNode newLevel = map.LevelAtCoords(newCoords);
        if (newLevel == null)
        {
            Debug.Log("Error: no new level found on exit. Aborting...");
            return;
        }
        LoadLevel(newLevel.id, LevelNode.oppDir(dir));
    }

    
    // Update is called once per frame
    void Update()
    { 
    }
}
