
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActorBuffs : MonoBehaviour
{
    public Actor actor;

    private Dictionary<string, BuffRuntimeData> staticBuffs = new();

    private Dictionary<string, BuffRuntimeData> dynamicBuffs = new();

    private void AddStaticBuff(string buffId, BuffData buffData)
    {
        if (staticBuffs.TryGetValue(buffId, out var buffRuntime))
        {
            buffRuntime.AddStack();
        }
        else
        {
            staticBuffs[buffId] = buffData.CreateRuntimeData(actor);
        }
    }

    private void RemoveStaticBuff(string buffId)
    {
        if (staticBuffs.TryGetValue(buffId, out var buffRuntime))
        {
            buffRuntime.RemoveStack();
            if (buffRuntime.NeedRemove())
            {
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
            existingBuff.Restart();
        }
        else
        {
            dynamicBuffs[buffId] = buffData.CreateRuntimeData(actor);
        }
    }

    private void RemoveDynamicBuff(string buffId)
    {
        if (dynamicBuffs.ContainsKey(buffId))
        {
            var buffRuntime = dynamicBuffs[buffId];
            dynamicBuffs.Remove(buffId);
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

    public IEnumerable<BuffRuntimeData> GetAllBuffs()
    {
        foreach (var item in staticBuffs.Values)
        {
            yield return item;
        }
        foreach (var item in dynamicBuffs.Values)
        {
            yield return item;
        }
    }

    public void DoUpdate(float deltaTime)
    {
        foreach (var item in staticBuffs)
        {
            item.Value.DoUpdate(deltaTime);
        }
        foreach (var item in dynamicBuffs.ToList())
        {
            item.Value.DoUpdate(deltaTime);
            if (item.Value.NeedRemove())
            {
                dynamicBuffs.Remove(item.Key);
                item.Value.Destroy();
            }
        }
    }

}
