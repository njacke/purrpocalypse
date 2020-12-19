using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSpawner : MonoBehaviour
{

    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0;


    public IEnumerator SpawnAllWaves()
    {
        for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++)
        {
            var currentWave = waveConfigs[waveIndex];
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
        }
    }

    public IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        for (int enemyCount = 0; enemyCount < waveConfig.GetNumberOfEnemies(); enemyCount++)
        {
            if (FindObjectOfType<Boss>().BossAlive() == true)
            {
                var newEnemy = Instantiate(
                waveConfig.GetEnemyPrefab(),
                GameObject.Find("Boss").transform.position,
                Quaternion.identity);
                newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
            }

        }
    }

}
