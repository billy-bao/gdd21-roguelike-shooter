using UnityEngine;
using System.Collections;

public class LevelData : MonoBehaviour
{
    public string sceneName;
    public Transform topExit;
    public Transform bottomExit;
    public Transform leftExit;
    public Transform rightExit;
    public Transform itemSpawn;
    public Item[] itemDrops;
    public WaveManager enemySpawns;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
