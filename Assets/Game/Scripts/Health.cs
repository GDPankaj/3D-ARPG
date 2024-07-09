using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]int _maxHealth;
    int _currentHealth = 0;
    Character _character;
    float _currentHealthRatio;

    private void Awake()
    {
        _character = GetComponent<Character>();
        _currentHealth = _maxHealth;
    }

    public int GetCurrentHealth()
    {
        return _currentHealth;
    }

    public void ApplyDamage(int damage)
    {
        _currentHealth -= damage;
        Debug.Log($"{gameObject.name} took damage of {damage}. Now its current health is {_currentHealth}");
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (_currentHealth <= 0 ) 
        {
            _character.SwitchStateTo(Character.CharacterState.Dead);
        }
    }

    public void AddHealth(int heal)
    {
        _currentHealth += heal;
        if(_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
        Debug.Log(_currentHealth);
    }

    public float GetHealthRatio()
    {
        _currentHealthRatio = (float)_currentHealth / (float)_maxHealth;
        return _currentHealthRatio;
    }

}
