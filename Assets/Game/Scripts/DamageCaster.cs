using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageCaster : MonoBehaviour
{
    Collider _damageCasterCollider;
    [SerializeField] int _damage;
    [SerializeField] string _targetTag;

    List<Collider> _damagedTargetList;


    private void Awake()
    {
        _damageCasterCollider = GetComponent<Collider>();
        _damageCasterCollider.enabled = false;
        _damagedTargetList = new List<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(_targetTag) && !_damagedTargetList.Contains(other))
        {
            Character character = other.gameObject.GetComponent<Character>();

            character?.ApplyDamage(_damage, transform.parent.position);

            GameVFXManager playerVfxManager = transform.parent.GetComponent<GameVFXManager>();

            if(playerVfxManager != null)
            {
                RaycastHit hit;

                Vector3 originalPos = transform.position + (-_damageCasterCollider.bounds.extents.z) * Vector3.forward;

                bool isHit = Physics.BoxCast(originalPos,
                    _damageCasterCollider.bounds.extents/2,
                    transform.forward,
                    out hit,
                    transform.rotation,
                    _damageCasterCollider.bounds.extents.z,
                    1 << 6);

                if (isHit)
                {
                    playerVfxManager.SlashVFX(hit.point + new Vector3(0, .5f, 0));
                }
            }
            _damagedTargetList?.Add(other);
        }
    }

    public void EnableDamageCaster()
    {
        _damagedTargetList.Clear();
        _damageCasterCollider.enabled = true;
    }

    public void DisableDamageCaster()
    {
        _damagedTargetList.Clear();
        _damageCasterCollider.enabled = false;
    }

    private void OnDrawGizmos()
    {
        if(_damageCasterCollider == null)
        {
            _damageCasterCollider = GetComponent<Collider>();
        }

        RaycastHit hit;

        Vector3 originalPos = transform.position + (-_damageCasterCollider.bounds.extents.z) * Vector3.forward;

        bool isHit = Physics.BoxCast(originalPos, 
            _damageCasterCollider.bounds.extents/2, 
            transform.forward, 
            out hit, 
            transform.rotation, 
            _damageCasterCollider.bounds.extents.z, 
            1 << 6);

        if(isHit)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(hit.point, 0.3f);
        }
    }
}
