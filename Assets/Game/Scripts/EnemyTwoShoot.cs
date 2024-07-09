using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTwoShoot : MonoBehaviour
{
    [SerializeField] Transform _shootingPoint;
    [SerializeField] GameObject _damageOrb;
    Character _character;

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    public void ShootTheDamageOrb()
    {
        Instantiate(_damageOrb, _shootingPoint.position, Quaternion.LookRotation(_shootingPoint.transform.forward));
    }

    private void Update()
    {
        _character.RotateToTarget();
    }
}
