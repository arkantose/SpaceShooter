using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaveManager : MonoBehaviour
{
    /* enemiesRemaining has to be static for it to make it easily accessabile in the enemy class*/
    public int enemiesRemaining = 0;
    [SerializeField]
    GameObject _enemiesContainer;
    [SerializeField]
    Wave[] _waves;
    private int _currentWave;
    bool _canSpawn = true;
    UIManager uiManager;
    // Start is called before the first frame update
    void Start()
    {
        uiManager = GameObject.Find("Game_Manager").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWaveInformation();
    }
    public void StartWaves()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2f);

        while (_canSpawn)
        {
            for (int i = _waves.Length - 1; i >= 0; i--)
            {
                _currentWave = i;
                enemiesRemaining = _waves[i].enemyCountPerWave;
                UpdateWaveAnnouncement();
                while (enemiesRemaining > 0)
                {
                    if (_waves[i].enemyCountPerWave > 0)
                    {
                        var posToSpawn = new Vector3(Random.Range(-9f, 9f), 8f, 0);
                        GameObject[] enemies = _waves[i].enemyTypesToSpawn;
                        GameObject spawn = enemies[Random.Range(0, enemies.Length)];
                        var enemy = Instantiate(spawn, posToSpawn, Quaternion.identity);
                        enemy.transform.parent = _enemiesContainer.transform;
                        _waves[i].enemyCountPerWave--;
                    }

                    //_waves[i].enemyCountPerWave--;

                    yield return new WaitForSeconds(_waves[i].enemySpawnTime);
                }

                 if (_currentWave == 0 && enemiesRemaining == 0)
                 {
                    uiManager.YouveWon();
                 }
                 yield return new WaitForSeconds(2f);
             }
            }
        }

    public void UpdateWaveAnnouncement()
    {
        uiManager.UpdateWaveUI(_currentWave + 1);
    }
    public void UpdateWaveInformation()
    {
        uiManager.CurrentWaveInfoUI(_currentWave , enemiesRemaining);
    }    


    public void UpdateEnemyCount()
    {
        enemiesRemaining--;
    }
}


    [System.Serializable]
    public class Wave
    {
        public GameObject [] enemyTypesToSpawn;
        public string waveName;
        public int enemyCountPerWave;
        public bool spawnBoss = false;
        public float enemySpawnTime = 5f;
        public AudioClip waveStartClip = null;
    }
