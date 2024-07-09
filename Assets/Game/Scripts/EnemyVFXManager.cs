using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyVFXManager : MonoBehaviour
{
    [SerializeField] VisualEffect _burstFootStep;
    [SerializeField] VisualEffect _smashVFX;
    [SerializeField] ParticleSystem _beingHitParticle;

    public void BurstFootStep()
    {
        _burstFootStep.Play();
    }

    public void SmashVFX()
    {
        //can also use this to play vfx
        _smashVFX.SendEvent("OnPlay");
    }

    public void BeingHit(Vector3 attackerPos)
    {
        Vector3 forceForward = transform.position - attackerPos;
        Vector3.Normalize(forceForward);
        forceForward.y = 0f;
        _beingHitParticle.transform.rotation = Quaternion.LookRotation(forceForward);
        _beingHitParticle.Play();
    }
}
