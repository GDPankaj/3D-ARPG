using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] GameObject _enemyToSpawn;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 centre = transform.position + new Vector3(0,.5f,0);
        Gizmos.DrawWireCube(centre, Vector3.one);
        Gizmos.DrawLine(centre, centre + transform.forward*2);
    }

    public GameObject EnemyToSpawn()
    {
        return _enemyToSpawn;
    }
}
