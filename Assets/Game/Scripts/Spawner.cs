using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    List<SpawnPoint> _spawnPointList;
    List<Character> _charcterList;
    bool _hasSpawned;
    [SerializeField]BoxCollider _collider;
    public UnityEvent AllSpawnedCharacterEliminated;

    private void Awake()
    {
        var spawnPointArray = transform.parent.GetComponentsInChildren<SpawnPoint>();
        _spawnPointList = new List<SpawnPoint>(spawnPointArray);
        _charcterList = new List<Character>();
    }

    private void Update()
    {
        if (!_hasSpawned || _charcterList.Count == 0)
        {
            return;
        }

        bool areAllEnemiesDead = true;

        foreach (var c in _charcterList)
        {
            if (c.GetCurrentState() != Character.CharacterState.Dead)
            {
                areAllEnemiesDead = false;
                break;
            }
        }

        if (areAllEnemiesDead) 
        {
            if(AllSpawnedCharacterEliminated != null)
            AllSpawnedCharacterEliminated.Invoke();

            _charcterList.Clear();
        }
        
    }

    public void SpawnCharacters()
    {
        if (_hasSpawned) return;

        _hasSpawned = true;

        foreach (var spawnPoint in _spawnPointList)
        {
            if (spawnPoint.EnemyToSpawn() != null)
            {
                GameObject spawnedEnemy =  Instantiate(spawnPoint.EnemyToSpawn(), spawnPoint.transform.position, Quaternion.identity);
                _charcterList.Add(spawnedEnemy.GetComponent<Character>());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SpawnCharacters();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(transform.position, _collider.bounds.size);
    }
}
