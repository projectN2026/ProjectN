using System;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager 
{
    private Stack<IPopupUI> _popupStack = new Stack<IPopupUI>();

    public T ShowPopup<T>(string prefabPath) where T : IPopupUI
    {
        var name = typeof(T).Name;
        var prefab = Resources.Load<GameObject>(prefabPath);
        if (prefab == null)
            Debug.LogWarning($"{prefabPath}경로에서 프리펩을 로드 할 수 없음");

        var go = GameObject.Instantiate(prefab);
        var popup = go.GetComponent<T>();
        if (popup == null)
            Debug.LogWarning($"{prefab.name}프리펩의 인스턴스에 {typeof(T).Name}컴포넌트가 존재하지 않음");

        _popupStack.Push(popup);

        return popup;
    }

    public void ClosePopup()
    {
        if (_popupStack.Count == 0)
            return;

        var popup = _popupStack.Pop();
        GameObject.Destroy(popup.GameObject);
    }
}
