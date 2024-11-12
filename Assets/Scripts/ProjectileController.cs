using System;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public GameObject explosionPrefab;
    public float projectileSpeed;
    public SpriteRenderer spriteRenderer;
    
    private Vector2 _direction;
    private float _damage;
    
    private bool _homing;
    private float _homingSmoothing;
    private GameObject[] _enemies;
    private Transform _target;
    
    private float _momentum;
    private float _timeAlive;

    private bool _volatile;
    private float _volatileRadius;
    private float _volatileDamage;
    
    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
    }
    
    public void SetColour(Color colour) => spriteRenderer.color = colour;
    public void SetDamage(float damage) => _damage = damage;
    public void SetMomentum(float momentum) => _momentum = momentum;

    public void SetVolatile(bool active, float radius, float damage)
    {
        _volatile = active;
        _volatileRadius = radius;
        _volatileDamage = damage;
    }
    
    private Transform FindNearestEnemy()
    {
        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject enemy in _enemies)
        {
            if (!enemy) continue;
            
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            
            if (distance > nearestDistance) continue;
            
            nearestDistance = distance;
            nearestEnemy = enemy;
        }

        return nearestEnemy ? nearestEnemy.transform : null;
    }

    public void SetHoming(bool homing, float smoothing)
    {
        _homing = homing;
        
        if (!homing) return;

        _homingSmoothing = smoothing;
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (_enemies.Length == 0)
        {
            _homing = false;
        }
    }
    
    private Rigidbody2D _rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (_homing) _target = FindNearestEnemy();
        _timeAlive += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (_homing && _target)
        {
            Vector3 targetDirection = (_target.position - transform.position).normalized;
            _direction = Vector3.Lerp(_direction, targetDirection, _homingSmoothing * Time.fixedDeltaTime).normalized;
        
            float angle = Mathf.Atan2(_direction.x, _direction.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
        }
        _rb.linearVelocity = _direction * projectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag("Enemy"))
        {
            if (!_volatile)
            {
                other.GetComponent<EnemyController>().Damage(_damage * Mathf.Pow(_momentum, projectileSpeed * _timeAlive));
            }
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (!_volatile) return;
        
        Explosion explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity).GetComponent<Explosion>();
        explosion.SetRadius(_volatileRadius);
        explosion.Damage = _volatileDamage * Mathf.Pow(_momentum, projectileSpeed * _timeAlive);
    }
}
