using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerCharacter player;
    [SerializeField] private List<EnemySpawner> enemySpawners;

    private float gameTime = 0f;

    private const float enemyUpgradeInterval = 15f;
    private const float playerUpgradeInterval = 30f;

    private float nextEnemyUpgrade = enemyUpgradeInterval;
    private float nextPlayerUpgrade = playerUpgradeInterval;

    void Update()
    {
        gameTime += Time.deltaTime;

        if (gameTime >= nextEnemyUpgrade)
        {
            UpgradeEnemies();
            nextEnemyUpgrade += enemyUpgradeInterval;
        }
        if (gameTime >= nextPlayerUpgrade)
        {
            UpgradePlayer();
            nextPlayerUpgrade += playerUpgradeInterval;
        }
    }

    private void UpgradeEnemies()
    {
        foreach (var spawner in enemySpawners)
        {
            foreach (var enemy in spawner.GetActiveEnemies()) 
            {
                enemy.UpgradeStats();
            }
        }
    }

    private void UpgradePlayer()
    {
        player.UpgradeStats();
    }
}

