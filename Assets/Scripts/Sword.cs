using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public float swordDamage = 40f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Miniboss"))
        {
            EnemyAI enemy = collision.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(swordDamage);
            }
        }
    }
}

