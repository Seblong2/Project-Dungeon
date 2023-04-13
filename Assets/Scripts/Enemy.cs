using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _enemyMinHealth;
    [SerializeField] private int _enemyMaxHealth;
    public int enemyHealth;


    void Start()
    {
        enemyHealth = Random.Range(_enemyMinHealth, _enemyMaxHealth);
    }

   

    public void TakeDamage(int damage)
    {
        // _audioSource.PlayOneShot(_hit);
        enemyHealth -= damage;

        if (enemyHealth <= 0)
        {

            Destroy(gameObject);

        }
    }
}
