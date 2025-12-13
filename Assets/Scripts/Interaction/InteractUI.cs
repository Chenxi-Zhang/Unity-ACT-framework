
using TMPro;
using UnityEngine;

public class InteractUI : MonoBehaviour
{
    public TextMeshProUGUI key;
    public TextMeshProUGUI desc;

    public void SetDisplayState(DisplayState state)
    {
        switch (state)
        {
            case DisplayState.Focus:
                key.SetText("E");
                desc.SetText("Interact");
                gameObject.SetActive(true);
                break;
            case DisplayState.Show:
                key.SetText("");
                desc.SetText("");
                gameObject.SetActive(true);
                break;
            case DisplayState.Hide:
                gameObject.SetActive(false);
                break;
        }
    }
}

public enum DisplayState
{
    Hide,
    // 屏幕中要显示，但是不能响应交互
    Show,
    // 屏幕中的主要目标，只有它能响应交互
    Focus,
}