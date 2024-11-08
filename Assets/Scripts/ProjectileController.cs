using System;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float projectileSpeed;
    
    private Vector2 _direction;
    public Vector2 Direction  {
        set => _direction = value;
    }
    
    private Rigidbody2D _rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        _rb.linearVelocity = _direction * projectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
    }
}
