using System;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager 
{
    private int _order = 0;
    private Stack<PopupBase> _popupStack = new Stack<PopupBase>();

    public T ShowPopup<T>() where T : PopupBase
    {
        var name = typeof(T).Name;
        var prefab = Resources.Load<GameObject>($"Prefabs/UI/{name}");
        var go = GameObject.Instantiate(prefab);
        var popup = go.GetComponent<T>();
        _popupStack.Push(popup);
        _order++;

        return popup;
    }

    public void ClosePopup()
    {
        if (_popupStack.Count == 0)
            return;

        var popup = _popupStack.Pop();
        GameObject.Destroy(popup.gameObject);
        _order--;
    }
}
