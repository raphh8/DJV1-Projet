using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour, IDamageable
{
    [SerializeField] private waterBullet waterBulletPrefab;
    [SerializeField] private int stressLvl = 0;
    [SerializeField] private int maxStress = 100;
    [SerializeField] private float shootDelay = 0.5f;

    private float _shootTimer;
    private MovementManager manager;

    void Update()
    {
        _shootTimer -= Time.deltaTime;
        if (manager.currentState is ShootingState && _shootTimer <= 0f)
        {
            Debug.Log("x");
            var direction = transform.rotation * Vector3.forward;
            Instantiate(waterBulletPrefab, transform.position + direction * 0.5f, transform.rotation).gameObject.SetActive(true);
            _shootTimer = shootDelay;
        }
    }

    public void ApplyDamage(int value)
    {
        stressLvl += value;

        if (stressLvl >= maxStress)
        {
            Destroy(gameObject);
        }
    }
}
