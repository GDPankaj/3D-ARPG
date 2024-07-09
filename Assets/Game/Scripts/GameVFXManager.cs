using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GameVFXManager : MonoBehaviour
{
    [SerializeField] VisualEffect _footStepVFX;
    [SerializeField] ParticleSystem _blade01VFX;
    [SerializeField] ParticleSystem _blade02VFX;
    [SerializeField] ParticleSystem _blade03VFX;
    [SerializeField] VisualEffect _slashVFX;
    [SerializeField] VisualEffect _healVFX;

    public void Update_FootStep(bool state)
    {
        if (state)
        {
            _footStepVFX.Play();
        }
        else
        {
            _footStepVFX.Stop();
        }
    }

    public void Blade01VFX()
    {
        _blade01VFX.Play();
    }

    public void Blade02VFX()
    {
        _blade02VFX.Play();
    }

    public void Blade03VFX()
    {
        _blade03VFX.Play();
    }

    public void StopBlade()
    {
        _blade01VFX.Simulate(0);
        _blade01VFX.Stop();
        _blade02VFX.Simulate(0);
        _blade02VFX.Stop();
        _blade03VFX.Simulate(0);
        _blade03VFX.Stop();
    }

    public void SlashVFX(Vector3 pos)
    {
        _slashVFX.transform.position = pos;
        _slashVFX.Play();
    }

    public void HealVFX()
    {
        _healVFX.Play();
    }
}
