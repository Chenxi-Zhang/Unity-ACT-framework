
using UnityEngine;

public class ParticleSystemSfx : BaseSfxContainer
{
    public ParticleSystem rootPs;

    public override void Destroy(float delay)
    {
        if (rootPs == null)
            return;
        rootPs.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        Destroy(rootPs.gameObject, delay);
    }
}