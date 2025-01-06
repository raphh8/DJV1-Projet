using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyCharacter : MonoBehaviour, IDamageable
{
    [SerializeField] private int initialHitPoints = 10;
    [SerializeField] private float moveCooldown = 3f;
    [SerializeField] private float angularSpeed = 360f;
    [SerializeField] private Transform target;
    [SerializeField] private ParticleSystem explosionPrefab;

    private int _hitPoints = 0;
    private float _moveTimer = 0f;
    private NavMeshAgent _navMeshAgent;
    private UnityEvent<EnemyCharacter> _onDestroy = new();
    private RaycastHit[] _raycastHits = new RaycastHit[2];

    Terrain terrain;
    Vector3 terrainSize;


    public float HitPointPercent => (float)_hitPoints / initialHitPoints;

    protected void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _hitPoints = initialHitPoints;

        terrain = Terrain.activeTerrain;
        terrainSize = terrain.terrainData.size;

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            transform.position = hit.position; 
        }
    }

    protected void OnEnable()
    {
        StartCoroutine(Coroutine());
        IEnumerator Coroutine()
        {
            yield return null;
            _navMeshAgent.enabled = true;

            while (enabled)
            {

                if (_navMeshAgent.isOnNavMesh)
                {

                    Vector3 terrainPosition = terrain.GetPosition();
                    float x = Random.Range(terrainPosition.x, terrainPosition.x + terrainSize.x);
                    float z = Random.Range(terrainPosition.z, terrainPosition.z + terrainSize.z);
                    _navMeshAgent.SetDestination(new Vector3(x, 0f, z));
                }
                else
                {
                    Debug.LogWarning("NavMeshAgent is not on a NavMesh. Skipping SetDestination.");
                }

                do yield return null;
                while (_navMeshAgent.hasPath);

                do
                {

                    var direction = (target.position - transform.position);
                    var lookRotation = Quaternion.LookRotation(direction.normalized);

                    transform.rotation = Quaternion.RotateTowards(
                        transform.rotation,
                        lookRotation,
                        angularSpeed * Time.deltaTime);

                    bool hitWall = false;

                    hitWall = Physics.RaycastNonAlloc(transform.position + Vector3.up, direction.normalized, _raycastHits, direction.magnitude) > 1;

                    yield return null;
                    _moveTimer += Time.deltaTime;
                } while (_moveTimer < moveCooldown);

                _moveTimer = 0f;
            }
        }
    }

    public void AddDestroyListener(UnityAction<EnemyCharacter> listener)
    {
        _onDestroy.AddListener(listener);
    }

    public void ApplyDamage(int value)
    {
        _hitPoints -= value;

        if (_hitPoints <= 0)
        {
            var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            explosion.gameObject.SetActive(true);
            explosion.Play();
            Destroy(explosion.gameObject, explosion.main.duration);

            Destroy(gameObject);
            _onDestroy.Invoke(this);
        }
    }
}
