
using System;
using System.Collections.Generic;
using UnityEngine;

public class StatusBuffs : MonoBehaviour
{
    private List<StatusBuff> buffUIs = new();

    void Awake()
    {
        buffUIs.AddRange(GetComponentsInChildren<StatusBuff>());
        for (int i = 0; i < buffUIs.Count; i++)
        {
            buffUIs[i].gameObject.SetActive(false);
        }
    }

    public void DoUpdate(Actor actor)
    {
        var buffs = actor.buffs;
        List<BuffRuntimeData> buffRuntimes = new();
        foreach (var item in buffs.GetAllBuffs())
        {
            if (item.buff.statusIcon != null)
            {
                buffRuntimes.Add(item);
            }
        }
        buffRuntimes.Sort(BuffComparer);
        for (int i = 0; i < buffUIs.Count; i++)
        {
            var ui = buffUIs[i];
            if (i < buffRuntimes.Count)
            {
                ui.gameObject.SetActive(true);
                ui.SetBuff(buffRuntimes[i]);
            }
            else
            {
                ui.gameObject.SetActive(false);
            }
        }
    }

    private int BuffComparer(BuffRuntimeData x, BuffRuntimeData y)
    {
        if (x.buff.IsStatic && y.buff.IsStatic)
        {
            return x.buff.buffId.CompareTo(y.buff.buffId);
        }
        else if (x.buff.IsStatic)
        {
            return -1;
        }
        else if (y.buff.IsStatic)
        {
            return 1;
        }
        else
        {
            return x.buff.buffId.CompareTo(y.buff.buffId);
        }
    }
}