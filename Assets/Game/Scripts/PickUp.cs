using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public enum PickUptype
    {
        Heal, Coin
    }

    [SerializeField] PickUptype type;
    [SerializeField] int value;
    [SerializeField] ParticleSystem collectedVFX;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Character>().PickUpItem(this);
            if(collectedVFX != null)
            {
                Instantiate(collectedVFX, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    public int GetValue()
    {
        return value;
    }
    public PickUptype GetPickUpType()
    {
        return type;
    }
}
