using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {

    public enum SpawnState { spawning, waiting, counting }

    [System.Serializable]
    public class Wave {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    private int nextWave = 0;
    public int NextWave {
        get { return nextWave; }
    }
    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;
    private float waveCountdown;
    public float WaveCountdown {
        get { return waveCountdown;   }
    }

    private float searchCountdown = 1f;

    private SpawnState state = SpawnState.counting;

    public SpawnState State {
        get { return state; }
    }

    void Start() {
        waveCountdown = timeBetweenWaves;
        if (spawnPoints.Length == 0) {
            Debug.LogError("No spawnpoints found");
        }
    }

    void Update() {
        if  (state == SpawnState.waiting) {
            if (!EnemyIsAlive()) {
                WaveCompleted();
            } else {
                return;
            }
        }

        if(waveCountdown <= 0) {
            if (state != SpawnState.spawning) {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        } else {
            waveCountdown -= Time.deltaTime;
        }
    }

    void WaveCompleted() {
        Debug.Log("Wave completed!");

        state = SpawnState.counting;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1) {
            nextWave = 0;
            Debug.Log("All waves complete! Looping...");
        } else {
            nextWave++;
        }
        
    }

    bool EnemyIsAlive() {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f) {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null) {
                return false;
            }
        }
        return true;
    }
     
    IEnumerator SpawnWave(Wave _wave) {
        Debug.Log("Spawning wave: " + _wave.name);
        state = SpawnState.spawning;

        for(int i = 0; i < _wave.count; i++) {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f/_wave.rate);
        }

        state = SpawnState.waiting;

        yield break;
    }

    void SpawnEnemy(Transform _enemy) {
        Debug.Log("Spawning enemy: " + _enemy.name);

        
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);
    }

}
