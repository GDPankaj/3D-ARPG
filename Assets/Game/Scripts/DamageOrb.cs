using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOrb : MonoBehaviour
{
    [SerializeField] float _speed = 2f;
    [SerializeField] int _damage = 10;

    [SerializeField] ParticleSystem _hitVFX;
    Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_rb != null) 
        {
            _rb.MovePosition(transform.position + transform.forward * _speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Character _cc = other.GetComponent<Character>();
        if (_cc != null && _cc.IsPlayer()) 
        {
            _cc.ApplyDamage(_damage, transform.position);
        }

        Instantiate(_hitVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
