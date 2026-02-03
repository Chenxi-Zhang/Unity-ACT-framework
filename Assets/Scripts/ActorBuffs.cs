
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActorBuffs : MonoBehaviour
{
    public Actor actor;
    private ActorData Data => actor.data;

    private Dictionary<string, int> staticBuffCounts = new();
    private Dictionary<string, BuffData> staticBuffs = new();

    private Dictionary<string, float> dynamicBuffCountdown = new();
    private Dictionary<string, BuffData> dynamicBuffs = new();

    private void AddStaticBuff(string buffId, BuffData buffData)
    {
        if (staticBuffCounts.ContainsKey(buffId))
        {
            staticBuffCounts[buffId]++;
        }
        else
        {
            staticBuffCounts[buffId] = 1;
            staticBuffs[buffId] = buffData;
            BuffStart(buffData);
        }
    }

    private void RemoveStaticBuff(string buffId)
    {
        if (staticBuffCounts.ContainsKey(buffId))
        {
            staticBuffCounts[buffId]--;
            if (staticBuffCounts[buffId] <= 0)
            {
                var buffData = staticBuffs[buffId];
                staticBuffCounts.Remove(buffId);
                staticBuffs.Remove(buffId);
                BuffStop(buffData);
            }
        }
    }

    private void AddDynamicBuff(string buffId, BuffData buffData, float duration)
    {
        if (dynamicBuffs.ContainsKey(buffId))
        {
            var existingBuff = dynamicBuffs[buffId];
            if (existingBuff.canRefresh)
            {
                dynamicBuffCountdown[buffId] = duration;
            }
        }
        else
        {
            dynamicBuffs[buffId] = buffData;
            dynamicBuffCountdown[buffId] = duration;
            BuffStart(buffData);
        }
    }

    private void RemoveDynamicBuff(string buffId)
    {
        if (dynamicBuffs.ContainsKey(buffId))
        {
            var buffData = dynamicBuffs[buffId];
            dynamicBuffs.Remove(buffId);
            dynamicBuffCountdown.Remove(buffId);
            BuffStop(buffData);
        }
    }

    public void AddBuff(BuffData buffData)
    {
        if (buffData.IsStatic)
        {
            AddStaticBuff(buffData.buffId, buffData);
        }
        else
        {
            AddDynamicBuff(buffData.buffId, buffData, buffData.duration);
        }
    }

    public void RemoveBuff(BuffData buffData)
    {
        if (buffData.IsStatic)
        {
            RemoveStaticBuff(buffData.buffId);
        }
        else
        {
            RemoveDynamicBuff(buffData.buffId);
        }
    }

    public void DoUpdate(float deltaTime)
    {
        foreach (var kvp in dynamicBuffCountdown.ToList())
        {
            var buffId = kvp.Key;
            var newTime = kvp.Value - deltaTime;
            if (newTime <= 0)
                RemoveDynamicBuff(buffId);
            else
                dynamicBuffCountdown[buffId] = newTime;
        }

        foreach (var item in staticBuffs)
        {
            DoUpdateBuffEffect(item.Value, deltaTime);
        }
        foreach (var item in dynamicBuffs)
        {
            DoUpdateBuffEffect(item.Value, deltaTime);
        }
    }

    private void DoUpdateBuffEffect(BuffData buffData, float deltaTime)
    {
        if (buffData.addHpRatioDur > 0)
        {
            var addHp = Data.RuntimeData.maxHp * buffData.addHpRatioDur * deltaTime;
            Data.AddHp(addHp);
        }
    }

    private void BuffStart(BuffData buffData)
    {
        if (buffData.addHpRatioImmediately != 0)
        {
            var addHp = Data.RuntimeData.maxHp * buffData.addHpRatioImmediately;
            Data.AddHp(addHp);
        }
    }

    private void BuffStop(BuffData buffData)
    {
    }

}
