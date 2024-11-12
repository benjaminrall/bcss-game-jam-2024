using System;
using UnityEngine;

public class RockProjectileController : MonoBehaviour
{
    public float projectileSpeed;
    private Vector2 _direction;
    private float _damage;
    
    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
    }
    
    public void SetDamage(float damage) => _damage = damage;
    private Rigidbody2D _rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        _rb.linearVelocity = _direction * projectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Environment"))
        {
            Destroy(gameObject);
        } else if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().Damage(_damage);
            Destroy(gameObject);
        } else if (other.CompareTag("EnemyProjectile") || other.CompareTag("Projectile") )

        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
