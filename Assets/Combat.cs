using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    private BoxCollider hitbox;
    private ParticleSystem burst;
    [SerializeField] private Animator _weaponAnimation;

    public int damage;
    [SerializeField] AudioClip _swordSlash;
    [SerializeField] AudioSource _audioSource;

    void Start() //finds all of the components and sets the starting values
    {
        damage = 1;
        _weaponAnimation = transform.parent.gameObject.GetComponent<Animator>();
        hitbox = gameObject.GetComponent<BoxCollider>();
        hitbox.enabled = false;
        burst = gameObject.GetComponent<ParticleSystem>();
    }

    void FixedUpdate()  //gives the player i-frames and lets their attack linger
    {
        if (burst.isPlaying == false)
        {
            hitbox.enabled = false;
        }
    }
    
    public void Attack()  //when the player attacks, enables the hitbox, plays an animation and a particle effect
    {
        _weaponAnimation.SetTrigger("attack");
        _audioSource.PlayOneShot(_swordSlash);
        hitbox.enabled = true;
        burst.Play();
    }

    private void OnTriggerEnter(Collider other)  //if the attack hitbox hits an enemy, trigger their pain script
    {
        if (other.tag == "Foe")
        {
            other.GetComponent<Gelcube>().Pain(damage);
        }
        else if (other.tag == "Boss")
        {
            other.GetComponent<Boss>().TakeDamage(damage);
        }
    }
}
