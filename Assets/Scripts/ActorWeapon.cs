
using System;
using System.Collections.Generic;
using UnityEngine;

public class ActorWeapon : MonoBehaviour
{
    public Actor actor;
    public List<WeaponState> weapons = new();
    private WeaponState weaponNow;

    public List<string> weaponIds = new();

    void Awake()
    {
        foreach (var item in weapons)
        {
            foreach (var weaponObj in item.weapons)
            {
                weaponObj.SetActive(false);
            }
        }
        if (weaponIds.Count > 0)
        {
            weaponNow = weapons.Find(w => w.id == weaponIds[0]);
            SetWeapon(weaponNow);
        }
    }

    private void RemoveWeapon(WeaponState weaponState)
    {
        if (weaponState == null)
            return;
        foreach (var weapon in weaponState.weapons)
        {
            weapon.SetActive(false);
        }
        actor.actionPlayableDirector.inputActionMgr.RemoveMapping(weaponState.inputMapping);
        actor.beHit.shockTypeActionMappingManager.RemoveMapping(weaponState.hitShockMapping);
    }

    private void SetWeapon(WeaponState weaponState)
    {
        foreach (var weapon in weaponState.weapons)
        {
            weapon.SetActive(true);
        }
        actor.actionPlayableDirector.inputActionMgr.AddMapping(weaponState.inputMapping);
        actor.beHit.shockTypeActionMappingManager.AddMapping(weaponState.hitShockMapping);
        // 切换武器强制播放Idle，主要打断当前模组动作，理论上应该播放，切换时的动作，在下一个动作模组中对应的那一个.以后再完善
        actor.actionPlayableDirector.PlayIdle();
    }

    public void EquipWeapon(string weaponId)
    {
        var weaponState = weapons.Find(w => w.id == weaponId);
        if (weaponState != null)
        {
            RemoveWeapon(weaponNow);
            SetWeapon(weaponState);
            weaponNow = weaponState;
        }
    }

    public void EquipNextWeapon()
    {
        if (weaponIds.Count == 0)
            return;
        var currentIndex = weaponIds.IndexOf(weaponNow.id);
        var nextIndex = (currentIndex + 1) % weaponIds.Count;
        EquipWeapon(weaponIds[nextIndex]);
    }

}

[Serializable]
public class WeaponState
{
    public string id;
    public List<GameObject> weapons;
    public InputTypeActionMapping inputMapping;
    public ShockTypeActionMapping hitShockMapping;
}