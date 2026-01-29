
using System;
using System.Collections.Generic;
using UnityEngine;

// One attack checker. This class helps to avoid duplicate hits on the same target.
public class ActorAttacker : MonoBehaviour
{
    public Actor actor;

    [NonSerialized]
    public WeaponState weaponState;
    [NonSerialized]
    public AttackData attackData;
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
        attackData = null;
    }

    public void TryHit(ActorBeHit hittable)
    {
        if (IsHit(hittable))
            return;
        AddHitbox(hittable);
        hittable.BeHit(this);
    }

    // 角色被对方Counter时调用
    public void BeCounter(Actor enemy, AttackBeCounterData attackBeCounterData, DefenceData defenceData)
    {
        actor.logicInput.InputForceAction(InputType.ForceActionBeCounter, attackBeCounterData.beCounterAction);
    }

    public float GetDamage()
    {
        if (weaponState == null)
            return 0f;
        return weaponState.weaponData.baseDamage * attackData.damageMultiplier;
    }
}
