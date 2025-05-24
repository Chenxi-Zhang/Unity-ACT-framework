
using System;
using System.Collections.Generic;
using UnityEngine;

// One attack checker. This class helps to avoid duplicate hits on the same target.
public class ActorAttacker : MonoBehaviour
{
    public Actor actor;
    [LayerField]
    public int attackLayer = 0;

    private readonly HashSet<IHittable> hitCheckers = new();

    void AddHitbox(IHittable hitbox)
    {
        hitCheckers.Add(hitbox);
    }

    bool IsHit(IHittable hitbox)
    {
        return hitCheckers.Contains(hitbox);
    }

    public void ClearHitboxes()
    {
        hitCheckers.Clear();
    }

    public void TryHit(ActorBeHit hittable)
    {
        if (IsHit(hittable))
            return;
        AddHitbox(hittable);
        hittable.BeHit(this);
    }

}
