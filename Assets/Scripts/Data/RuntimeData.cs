
using System;

[Serializable]
public class RuntimeData
{
    public int hp;
    public int maxHp;

    public void Add(RuntimeData other)
    {
        hp += other.hp;
        maxHp += other.maxHp;
    }

    public void Reduce(RuntimeData other)
    {
        hp -= other.hp;
        maxHp -= other.maxHp;
    }

}