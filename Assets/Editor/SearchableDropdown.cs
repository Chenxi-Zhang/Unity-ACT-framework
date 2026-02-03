
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SearchableDropdown : PopupWindowContent
{
    private string search = "";
    private Vector2 scroll;

    private List<string> items;
    private Action<string> onSelect;

    private static GUIStyle leftAlignedButton;

    public SearchableDropdown(List<string> items, Action<string> onSelect)
    {
        this.items = items;
        this.onSelect = onSelect;

        // 初始化靠左对齐的按钮样式
        if (leftAlignedButton == null)
        {
            leftAlignedButton = new GUIStyle(GUI.skin.button);
            leftAlignedButton.alignment = TextAnchor.MiddleLeft;
            leftAlignedButton.padding = new RectOffset(10, 10, 2, 2);
        }
    }

    public override Vector2 GetWindowSize()
    {
        return new Vector2(250, 300);
    }

    public override void OnGUI(Rect rect)
    {
        // 输入框
        search = EditorGUILayout.TextField(search);
        // 滚动区域
        scroll = EditorGUILayout.BeginScrollView(scroll);
        foreach (var item in items)
        {
            if (!string.IsNullOrEmpty(search) &&
                !item.ToLower().Contains(search.ToLower()))
                continue;
            if (GUILayout.Button(item, leftAlignedButton, GUILayout.Height(20)))
            {
                onSelect?.Invoke(item);
                editorWindow.Close();
            }
        }
        EditorGUILayout.EndScrollView();
    }
}
