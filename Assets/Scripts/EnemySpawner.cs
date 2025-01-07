using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private EnemyCharacter enemyPrefab;

    private int _enemyKilledCount = 0;
    private float _spawnTimer = 0f;
    private List<EnemyCharacter> _enemyCharacters = new();

    public int EnemyKilledCount => _enemyKilledCount;

    private IEnumerator Start()
    {
        Terrain terrain = Terrain.activeTerrain;
        Vector3 terrainPosition = terrain.transform.position;
        Vector3 terrainSize = terrain.terrainData.size;
        while (true)
        {
            _spawnTimer += Time.deltaTime;

            if (_spawnTimer > spawnInterval)
            {
                _spawnTimer = 0f;

                float x = Random.Range(terrainPosition.x, terrainPosition.x + terrainSize.x);
                float z = Random.Range(terrainPosition.z, terrainPosition.z + terrainSize.z);

                var enemy = Instantiate(enemyPrefab, new Vector3(x, terrainPosition.y, z), Quaternion.identity, transform);
                enemy.gameObject.SetActive(true);
                _enemyCharacters.Add(enemy);
                enemy.AddDestroyListener(OnEnemyCharacterDestroyed);
            }

            yield return null;
        }
    }

    private void OnEnemyCharacterDestroyed(EnemyCharacter enemy)
    {
        _enemyCharacters.Remove(enemy);
        _enemyKilledCount += 1;
    }
}
