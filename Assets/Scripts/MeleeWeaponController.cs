using System;
using UnityEngine;

public class MeleeWeaponController : MonoBehaviour
{
    public PlayerController player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy.Damage(player.GetMeleeDamage()))
            {
                player.MeleeKill();
            }
            else
            {
                player.StunEnemy(enemy);
            }
        }
    }
}
