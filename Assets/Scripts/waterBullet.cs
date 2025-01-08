using UnityEngine;

public class waterBullet : MonoBehaviour
{
    [SerializeField] private int damage = 6;
    [SerializeField] private float speed = 50f;

    private void FixedUpdate()
    {
        transform.position += transform.forward * (speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.ApplyDamage(damage);
        }
        Destroy(gameObject);
    }
}
