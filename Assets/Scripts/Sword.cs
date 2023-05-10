using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private Animator _weaponAnimation;
    private bool _isAttacking;

    public int damage;
    [SerializeField] AudioClip _swordSlash;
    [SerializeField] AudioSource _audioSource;


    public Transform attackPoint;
    public float weaponRange;
    public LayerMask enemyLayer;
    //[SerializeField] private Enemy enemy;
    

    public void Attack()
    {
        _isAttacking = true;
        _weaponAnimation.SetTrigger("attack");
        _audioSource.PlayOneShot(_swordSlash);
        Collider[] enemiesHit = Physics.OverlapSphere(attackPoint.position, weaponRange, enemyLayer);

        foreach (Collider enemy in enemiesHit)
        {
            enemy.GetComponent<Gelcube>().Pain(damage);
            Debug.Log("Hit enemy");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawSphere(attackPoint.position, weaponRange);
    }
}
