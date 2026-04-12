using System;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : SingletonBehaviour<PopupManager>
{
    private int _order = 0;
    private Stack<PopupBase> _popupStack = new Stack<PopupBase>();

    public static T ShowPopup<T>() where T : PopupBase
    {
        var name = typeof(T).Name;
        var prefab = Resources.Load<GameObject>($"Prefabs/UI/{name}");
        var go = Instantiate(prefab);
        var popup = go.GetComponent<T>();
        Instance._popupStack.Push(popup);
        Instance._order++;

        return popup;
    }

    public static void ClosePopup()
    {
        if (Instance._popupStack.Count == 0)
            return;

        var popup = Instance._popupStack.Pop();
        Destroy(popup.gameObject);
        Instance._order--;
    }
}
