using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Sprite[] sprites;
    public SpriteRenderer spriteRenderer;

    public float maxHealth;
    public float hitImmunityTime = 0.1f;

    private float _health;
    private float _immunityTimer;

    private IEnemyBehaviour _behaviour;
    private Rigidbody2D _rigidbody;
    
    public bool Damage(float damage)
    {
        if (_immunityTimer > 0) return false;
        
        _immunityTimer = hitImmunityTime;
        _health -= damage;
        if (_health > 0) return false;
        
        Destroy(gameObject);
        return true;
    }

    public void Stun(float duration)
    {
        
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _health = maxHealth;
        _behaviour = GetComponent<IEnemyBehaviour>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        spriteRenderer.sprite = sprites[_behaviour.GetSpriteIndex(sprites.Length)];
        
        if (_immunityTimer > 0)
        {
            _immunityTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.linearVelocity = _behaviour.GetMovement();
    }
}
