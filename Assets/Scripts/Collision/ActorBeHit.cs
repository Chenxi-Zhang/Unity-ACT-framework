
using System;
using System.Collections.Generic;
using UnityEngine;

public class ActorBeHit : MonoBehaviour, IHittable
{
    public Actor actor;

    public DefenceData hitDefenceData;
    [HideInInspector]
    public List<HitCounterDefenceData> hitCounters;

    // 根据攻击带有的hitCounter属性，查找角色在防御上是否有对应的hitCounter，从而获取对应的防御数据
    private bool TryGetHitCounterDefenceData(AttackData attackData, out HitCounterDefenceData hitCounterDefData, out AttackBeCounterData attackBeCounterData)
    {
        hitCounterDefData = null;
        attackBeCounterData = null;
        if (attackData.hitCounters == null)
            return false;
        for (int i = 0; i < attackData.hitCounters.Length; i++)
        {
            var attackHitCounter = attackData.hitCounters[i];
            if (string.IsNullOrEmpty(attackHitCounter.hitCounter.name))
                continue;
            for (int j = hitCounters.Count - 1; j >= 0; j--)
            {
                if (hitCounters[j].hitCounter.name == attackHitCounter.hitCounter.name)
                {
                    hitCounterDefData = hitCounters[j];
                    attackBeCounterData = attackHitCounter;
                    return true;
                }
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
        DefenceData defenceData;
        if (TryGetHitCounterDefenceData(attacker.attackData, out var hitCounterDefData, out var attackBeCounterData))
        {
            if (hitCounterDefData.ignoreHit)
            {
                return;
            }
            defenceData = hitCounterDefData.defenceData;
            attacker.BeCounter(actor, attackBeCounterData, defenceData);
        }
        else
        {
            defenceData = hitDefenceData;
        }
        var action = defenceData.GetActionFromShock(attacker.attackData.shockType);
        actor.logicInput.InputForceAction(InputType.ForceBeHit, action);
    }

}

[Serializable]
public class HitCounterDefenceData
{
    public HitCounterType hitCounter;
    public bool ignoreHit;
    public DefenceData defenceData;
}
