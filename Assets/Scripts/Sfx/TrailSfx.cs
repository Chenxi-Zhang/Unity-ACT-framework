
using UnityEngine;

public class TrailSfx : BaseSfxContainer
{
    public TrailRenderer trailRenderer;

    public override void Destroy(float delay)
    {
        trailRenderer.emitting = false;
        Destroy(gameObject, delay);
    }
}