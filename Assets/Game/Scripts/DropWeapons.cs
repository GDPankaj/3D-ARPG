using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropWeapons : MonoBehaviour
{
    [SerializeField] List<GameObject> weapons = new List<GameObject>();

    public void DropSwords()
    {
        foreach (GameObject weapon in weapons)
        {
            weapon.AddComponent<Rigidbody>();
            weapon.AddComponent<BoxCollider>();

            weapon.transform.SetParent(null);
        }
    }

}
