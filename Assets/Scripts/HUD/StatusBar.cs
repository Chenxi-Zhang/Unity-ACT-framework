
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    public Scrollbar hpBar;

    public void SetHpRatio(float ratio)
    {
        hpBar.size = Mathf.Clamp01(ratio);
    }

}