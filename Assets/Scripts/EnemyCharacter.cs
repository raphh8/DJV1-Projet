using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyCharacter : MonoBehaviour, IDamageable
{
    [SerializeField] private int initialLife = 6;
    [SerializeField] private float moveCooldown = 5f;
    [SerializeField] private float shootCooldown = 2f;
    [SerializeField] private float angularSpeed = 360f;
    [SerializeField] private ParticleSystem explosionPrefab;
    [SerializeField] public waterBullet waterBulletPrefab;
    [SerializeField] private Transform eyePoint;

    private int life = 0;
    private float moveTimer = 0f;
    private float shootTimer = 0f;
    private NavMeshAgent navMeshAgent;
    private UnityEvent<EnemyCharacter> onDestroy = new();
    private RaycastHit[] raycastHits = new RaycastHit[2];

    Terrain terrain;
    Vector3 terrainSize;

    [SerializeField] public PlayerCharacter player;
    private bool playerIsDead = false;
    private bool isDead;

    public float LifePercent => (float)life / initialLife;

    protected void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        life = initialLife;
        isDead = false;

    terrain = Terrain.activeTerrain;
        terrainSize = terrain.terrainData.size;

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            transform.position = hit.position; 
        }
    }

    protected void OnEnable()
    {
        if (player != null)
        {
            player.ApplyDamage(10); 
        }

        StartCoroutine(Coroutine());
        IEnumerator Coroutine()
        {
            yield return null;
            navMeshAgent.enabled = true;

            while (enabled && !isDead)
            {
                if (playerIsDead) yield break;
                if (navMeshAgent.isOnNavMesh)
                {
                    Vector3 terrainPosition = terrain.GetPosition();
                    float x = Random.Range(terrainPosition.x, terrainPosition.x + terrainSize.x);
                    float z = Random.Range(terrainPosition.z, terrainPosition.z + terrainSize.z);
                    navMeshAgent.SetDestination(new Vector3(x, 0f, z));
                }
                else
                {
                    Debug.LogWarning("NavMeshAgent is not on a NavMesh. Skipping SetDestination.");
                }

                do yield return null;
                while (navMeshAgent.hasPath);

                do
                {
                    if (playerIsDead) yield break;

                    shootTimer += Time.deltaTime;

                    Vector3 direction = (player.transform.position + Vector3.up * 1.5f - eyePoint.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(direction);

                    transform.rotation = Quaternion.RotateTowards(
                        transform.rotation,
                        lookRotation,
                        angularSpeed * Time.deltaTime);

                    bool hitWall = Physics.Raycast(transform.position + Vector3.up, direction.normalized, out RaycastHit hit, direction.magnitude, LayerMask.GetMask("Default", "Ground"));

                    yield return null;
                    moveTimer += Time.deltaTime;

                    if (shootTimer >= shootCooldown && !hitWall)
                    {
                        shootTimer = 0f; 
                        Vector3 spawnPosition = eyePoint.position;
                        Instantiate(waterBulletPrefab, spawnPosition, lookRotation).gameObject.SetActive(true);
                    }

                } while (moveTimer < moveCooldown);

                moveTimer = 0f;
                shootTimer = 0f;
            }
        }
    }

    public void UpgradeStats()
    {
        initialLife += 5;
        moveCooldown -= 0.1f;
        shootCooldown -= 0.1f;
        angularSpeed += 10f;
    }

    public void AddDestroyListener(UnityAction<EnemyCharacter> listener)
    {
        onDestroy.AddListener(listener);
    }

    public void ApplyDamage(int value)
    {
        if(isDead) return;

        life -= value;
        life = Mathf.Clamp(life, 0, initialLife);

        if (life <= 0)
        {
            isDead = true;
            player.ApplyDamage(-10);
            var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            explosion.gameObject.SetActive(true);
            explosion.Play();
            Destroy(explosion.gameObject, explosion.main.duration);

            Destroy(gameObject);
            onDestroy.Invoke(this);
        }
    }
    public void NotifyPlayerDead()
    {
        playerIsDead = true;
        navMeshAgent.isStopped = true;
    }
}
