
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    public Scrollbar hpBar;
    public StatusBuffs statusBuffs;

    public void SetHpRatio(float ratio)
    {
        hpBar.size = Mathf.Clamp01(ratio);
    }

    public void UpdateStatus(Actor actor)
    {
        statusBuffs.DoUpdate(actor);
    }

}