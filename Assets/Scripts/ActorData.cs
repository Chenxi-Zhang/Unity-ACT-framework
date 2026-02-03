
using System;
using UnityEngine;

public class ActorData : MonoBehaviour
{
    public Actor actor;
    public StatusIndicator statusIndicator;

    public bool IsAlive { get; private set; }

    [SerializeField]
    private RuntimeData staticData;
    private RuntimeData runtimeData;
    public RuntimeData RuntimeData => runtimeData;
    public WeaponData WeaponData => actor.weapon.WeaponNow?.weaponData;

    [SerializeField]
    private ItemState initialItem;
    private ItemState itemToUse;

    private float hpAccumulator = 0f;

    void Start()
    {
        statusIndicator.DoStart();
        Initialize();
        SwitchUseItem(initialItem);
    }

    public void Initialize()
    {
        IsAlive = true;
        runtimeData = new();
        runtimeData.maxHp = staticData.maxHp;
        SetHp(staticData.hp);
    }

    public void SetMaxHp(int maxHp)
    {
        runtimeData.maxHp = maxHp;
        if (runtimeData.hp > runtimeData.maxHp)
        {
            runtimeData.hp = runtimeData.maxHp;
        }
        statusIndicator.SetHpRatio((float)runtimeData.hp / runtimeData.maxHp);
    }

    public void SetHp(int hp)
    {
        if (!IsAlive)
        {
            return;
        }
        runtimeData.hp = hp;
        if (runtimeData.hp > runtimeData.maxHp)
        {
            runtimeData.hp = runtimeData.maxHp;
        }
        else if (runtimeData.hp < 0)
        {
            runtimeData.hp = 0;
            ProcessDeath();
        }
        statusIndicator.SetHpRatio((float)runtimeData.hp / runtimeData.maxHp);
    }

    private void ProcessDeath()
    {
        IsAlive = false;
        actor.logicInput.InputButton(InputType.Death);
    }

    public void AddHp(float value)
    {
        hpAccumulator += value;
        var intValue = Mathf.FloorToInt(hpAccumulator);
        if (intValue != 0)
        {
            SetHp(runtimeData.hp + intValue);
            hpAccumulator -= intValue;
        }
    }

    public void DoUpdate(float deltaTime)
    {
        statusIndicator.UpdateUIPosition();
    }

    public void SwitchUseItem(ItemState newItem)
    {
        if (itemToUse != null)
        {
            actor.actionPlayableDirector.inputActionMgr.RemoveMapping(itemToUse.ItemActionMapping);
        }
        itemToUse = newItem;
        if (itemToUse != null)
        {
            actor.actionPlayableDirector.inputActionMgr.AddMapping(itemToUse.ItemActionMapping);
        }
    }

}