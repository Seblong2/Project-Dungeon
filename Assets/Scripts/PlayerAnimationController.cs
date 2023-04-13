using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody _rb;
    private float _animationSpeed = 5f;

    void Start()
    {
        _animator = this.GetComponent<Animator>();
        _rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetFloat("speed", _rb.velocity.magnitude / _animationSpeed);
    }
}
