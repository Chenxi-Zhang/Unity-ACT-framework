
using UnityEngine;
using UnityEngine.UI;

public class StatusBuff : MonoBehaviour
{
    public Image icon;
    public Image durationFill;

    public void SetBuff(BuffRuntimeData buffRuntime)
    {
        icon.sprite = buffRuntime.buff.statusIcon;
        durationFill.fillAmount = buffRuntime.GetFillAmount();
    }

}