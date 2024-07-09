using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] GameObject _gateVisual;
    BoxCollider _gateCollider;
    [SerializeField] float _openGateDuration = 2f;
    [SerializeField] float _openTargetY = -1.5f;

    private void Awake()
    {
        _gateCollider = GetComponent<BoxCollider>();
    }

    IEnumerator OpenGateAnimation()
    {
        float currentOpenDuration = 0;
        Vector3 startPos = _gateVisual.transform.position;
        Vector3 targetPos = startPos + Vector3.up * _openTargetY;

        while (currentOpenDuration < _openGateDuration)
        {
            currentOpenDuration += Time.deltaTime ;
            _gateVisual.transform.position = Vector3.Lerp(startPos, targetPos, currentOpenDuration/_openGateDuration);
            yield return null;
        }
        _gateCollider.enabled = false;

    }


    public void Open()
    {
        StartCoroutine(OpenGateAnimation());
    }
}
