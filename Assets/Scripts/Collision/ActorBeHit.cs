
using System;
using UnityEngine;

public class ActorBeHit : MonoBehaviour, IHittable
{
    public Actor actor;

    public DefenceData hitDefenceData;
    public ShockTypeActionMappingManager shockTypeActionMappingManager = new();
    [HideInInspector]
    public HitCounterMappingManager hitCounterMappingManager = new();

    // 根据攻击带有的hitCounter属性，查找角色在防御上是否有对应的hitCounter，从而获取对应的防御数据
    private bool TryGetHitCounterDefenceData(AttackData attackData, out HitCounterInfo hitCounterData, out AttackBeCounterData attackBeCounterData)
    {
        hitCounterData = null;
        attackBeCounterData = null;
        if (attackData.hitCounters == null)
            return false;
        for (int i = 0; i < attackData.hitCounters.Length; i++)
        {
            var attackHitCounter = attackData.hitCounters[i];
            if (string.IsNullOrEmpty(attackHitCounter.hitCounter.name))
                continue;
            if (hitCounterMappingManager.TryGetValue(attackHitCounter.hitCounter, out var mapping))
            {
                hitCounterData = mapping;
                hitCounterData.hitCounter = attackHitCounter.hitCounter;
                attackBeCounterData = attackHitCounter;
                return true;
            }
        }
        return false;
    }

    public void BeHit(ActorAttacker attacker)
    {
        if (attacker.actor == actor)
            // don't hit self
            return;
        Debug.Log($"{attacker.gameObject.name} Hits {actor.gameObject.name}");
        ActionTimelineAsset action = null;
        if (TryGetHitCounterDefenceData(attacker.attackData, out var hitCounterData, out var attackBeCounterData))
        {
            if (hitCounterData.ignoreHit)
            {
                return;
            }
            if (hitCounterData.shockTypeActionMapping != null)
            {
                hitCounterData.shockTypeActionMapping.TryGetValue(attacker.attackData.shockType, out action);
            }
            attacker.BeCounter(actor, attackBeCounterData, hitCounterData.defenceData);
        }
        else
        {
            shockTypeActionMappingManager.TryGetValue(attacker.attackData.shockType, out action);
        }
        if (action != null)
        {
            actor.logicInput.InputForceAction(InputType.ForceBeHit, action);
        }
        HandleDamage(attacker, hitCounterData);
    }

    private void HandleDamage(ActorAttacker attacker, HitCounterInfo hitCounterData)
    {
        var damage = attacker.GetDamage();
        var finalDamage = GetFinalDamage(damage, hitCounterData);
        actor.data.AddHp(-finalDamage);
    }

    private float GetFinalDamage(float damage, HitCounterInfo hitCounterInfo)
    {
        if (hitCounterInfo == null)
            return damage;
        var weaponData = actor.data.WeaponData;
        if (weaponData == null)
            return damage;
        var hitCounterDefRatio = weaponData.GetCounterDefRatio(hitCounterInfo.hitCounter);
        return damage * (1f - hitCounterDefRatio);
    }
}
