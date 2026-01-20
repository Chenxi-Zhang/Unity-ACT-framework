
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActorBuffs : MonoBehaviour
{
    public Actor actor;

    private Dictionary<string, int> staticBuffCounts = new();
    private Dictionary<string, BuffRuntimeData> staticBuffs = new();

    private Dictionary<string, float> dynamicBuffCountdown = new();
    private Dictionary<string, BuffRuntimeData> dynamicBuffs = new();

    private void AddStaticBuff(string buffId, BuffData buffData)
    {
        if (staticBuffCounts.ContainsKey(buffId))
        {
            staticBuffCounts[buffId]++;
        }
        else
        {
            staticBuffCounts[buffId] = 1;
            staticBuffs[buffId] = buffData.CreateRuntimeData(actor);
        }
    }

    private void RemoveStaticBuff(string buffId)
    {
        if (staticBuffCounts.ContainsKey(buffId))
        {
            staticBuffCounts[buffId]--;
            if (staticBuffCounts[buffId] <= 0)
            {
                var buffRuntime = staticBuffs[buffId];
                staticBuffCounts.Remove(buffId);
                staticBuffs.Remove(buffId);
                buffRuntime.Destroy();
            }
        }
    }

    private void AddDynamicBuff(string buffId, BuffData buffData, float duration)
    {
        if (dynamicBuffs.ContainsKey(buffId))
        {
            var existingBuff = dynamicBuffs[buffId];
            if (existingBuff.buff.canRefresh)
            {
                dynamicBuffCountdown[buffId] = duration;
            }
        }
        else
        {
            dynamicBuffs[buffId] = buffData.CreateRuntimeData(actor);
            dynamicBuffCountdown[buffId] = duration;
        }
    }

    private void RemoveDynamicBuff(string buffId)
    {
        if (dynamicBuffs.ContainsKey(buffId))
        {
            var buffRuntime = dynamicBuffs[buffId];
            dynamicBuffs.Remove(buffId);
            dynamicBuffCountdown.Remove(buffId);
            buffRuntime.Destroy();
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
            item.Value.DoUpdate(deltaTime);
        }
        foreach (var item in dynamicBuffs)
        {
            item.Value.DoUpdate(deltaTime);
        }
    }

}
