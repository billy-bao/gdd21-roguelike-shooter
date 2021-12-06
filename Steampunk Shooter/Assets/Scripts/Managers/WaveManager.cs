using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public enum SpawnState {SPAWNING, WAITING, COUNTING, DONE };

    [System.Serializable]
    public class Wave
    {
        public string name;

        public GameObject enemy;

        public int amount;

        public float rate;
    }

    public Wave[] waves;
    private int nextwave = 0;

    public Transform[] spawnpoints;

    public float timeBetweenWaves = 5f;
    private float Wavecountdown;
    public SpawnState state = SpawnState.COUNTING;

    private float SearchCountDown = 1f;

    private void Start()
    {
        Wavecountdown = timeBetweenWaves;

        if(spawnpoints.Length == 0)
        {
            Debug.Log("ERROR: NO SPAWNPOINTS SET");
        }
    }

    private void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {
                //begin an new round
                WaveCompleted();
               

            } else
            {
                return;
            }
        }

        if (state == SpawnState.COUNTING)
        {
            if (Wavecountdown <= 0)
            {
                if (state != SpawnState.SPAWNING)
                {
                    //start spawning wave
                    StartCoroutine(SpawnWave(waves[nextwave]));
                }
            }
            else
            {
                Wavecountdown -= Time.deltaTime;
            }
        }
    }

    void WaveCompleted()
    {
        Debug.Log("Wave completed");

        state = SpawnState.COUNTING;

        Wavecountdown = timeBetweenWaves;

        if (nextwave + 1 > waves.Length - 1)
        {
            nextwave = 0;
            Debug.Log("All waves complete!");
            state = SpawnState.DONE;
        }
        else
        {
            nextwave++;
        }
        return;
    }


    bool EnemyIsAlive()
    {
        SearchCountDown -= Time.deltaTime;

        if (SearchCountDown <= 0)
        {
            SearchCountDown = 1f;
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                return false;
            }
            
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        state = SpawnState.SPAWNING;

        Debug.Log("Spawning wave" + _wave.name);

        for(int i = 0; i < _wave.amount; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1 / _wave.rate);
        }

        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnEnemy (GameObject _enemy)
    {
        Transform _sp = spawnpoints[Random.Range(0, spawnpoints.Length)];

        GameObject e = Instantiate(_enemy, _sp.position, _sp.rotation);
        e.tag = "Enemy";
        e.SetActive(true);
        Debug.Log("Spawning enemy at position: " + _sp.position.ToString());
    }

}
