using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnInterval = 7f;
    [SerializeField] private EnemyCharacter enemyPrefab;

    private int enemyKilledCount = 0;
    private float spawnTimer = 0f;
    private List<EnemyCharacter> enemyCharacters = new();

    public int EnemyKilledCount => enemyKilledCount;

    private IEnumerator Start()
    {
        Terrain terrain = Terrain.activeTerrain;
        Vector3 terrainPosition = terrain.transform.position;
        Vector3 terrainSize = terrain.terrainData.size;
        while (true)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer > spawnInterval)
            {
                spawnTimer = 0f;

                float x = Random.Range(terrainPosition.x, terrainPosition.x + terrainSize.x);
                float z = Random.Range(terrainPosition.z, terrainPosition.z + terrainSize.z);

                var enemy = Instantiate(enemyPrefab, new Vector3(x, terrainPosition.y, z), Quaternion.identity, transform);
                enemy.gameObject.SetActive(true);
                enemyCharacters.Add(enemy);
                enemy.AddDestroyListener(OnEnemyCharacterDestroyed);
            }

            yield return null;
        }
    }

    private void OnEnemyCharacterDestroyed(EnemyCharacter enemy)
    {
        enemyCharacters.Remove(enemy);
        enemyKilledCount += 1;
        enemy.player.OnEnemyKilled();
    }
}
