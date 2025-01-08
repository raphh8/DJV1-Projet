using UnityEngine;

public class UIScore : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private EnemySpawner enemySpawner;

    private void Update()
    {
        scoreText.text = $"Score : {enemySpawner.EnemyKilledCount}";
    }
}
