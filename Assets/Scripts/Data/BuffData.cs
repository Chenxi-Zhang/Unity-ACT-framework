
using System;

[Serializable]
public class BuffData
{

    public string buffId;
    public float duration;
    public bool IsStatic => duration <= 0f;
    public bool canRefresh;

    public float addHpRatioImmediately;
    public float addHpRatioDur;

    public SfxData sfx;

    public BuffRuntimeData CreateRuntimeData(Actor actor)
    {
        var data = actor.data;
        StartBuff(data);
        BuffRuntimeData buffRuntime = new ();
        buffRuntime.data = data;
        buffRuntime.buff = this;
        if (sfx != null)
            buffRuntime.sfxRuntime = sfx.CreateSfxFor(actor);
        return buffRuntime;
    }

    private void StartBuff(ActorData data)
    {
        if (addHpRatioImmediately != 0)
        {
            var addHp = data.RuntimeData.maxHp * addHpRatioImmediately;
            data.AddHp(addHp);
        }
    }
}

public class BuffRuntimeData
{
    public ActorData data;
    public BuffData buff;
    public SfxRuntimeData sfxRuntime;

    public void Destroy()
    {
        sfxRuntime?.Destroy();
        sfxRuntime = null;
    }

    public void DoUpdate(float deltaTime)
    {
        if (buff.addHpRatioDur > 0)
        {
            var addHp = data.RuntimeData.maxHp * (buff.addHpRatioDur * deltaTime);
            data.AddHp(addHp);
        }
    }
}