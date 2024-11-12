using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public float maxHealth;

    private float _health;

    public bool Damage(float damage)
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
