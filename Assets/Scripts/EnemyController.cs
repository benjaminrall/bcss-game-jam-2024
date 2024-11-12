using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public float maxHealth;

    private float _health;

    public void Damage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MeleeWeapon"))
        {
            Destroy(gameObject);
        }
    }
}
