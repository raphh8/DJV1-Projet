using UnityEngine;

public class UIEnemyHealth : MonoBehaviour
{
    [SerializeField] private RectTransform fillTransform;

    private EnemyCharacter enemyCharacter;
    private Camera camera;

    private void Awake()
    {
        enemyCharacter = GetComponentInParent<EnemyCharacter>();
        camera = Camera.main;
    }

    private void Update()
    {
        fillTransform.anchorMin = new Vector2(1f - enemyCharacter.LifePercent, 0f);
        transform.LookAt(camera.transform.position);
    }
}
