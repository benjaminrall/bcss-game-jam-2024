using System;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float Damage { get; set; }
    
    public void SetRadius(float radius)
    {
        transform.localScale = radius * Vector3.one;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().Damage(Damage);
        }
    }
}
