using UnityEngine;
using UnityEngine.UI;

public class ClosePopupButton : BaseBehaviour
{
    [SerializeField, ComponentField]
    private Button _button;

    protected override void OnInit()
    {
        base.OnInit();

        _button?.onClick?.AddListener(OnClick);
    }

    private void OnClick()
    {
        Managers.PopupManager.ClosePopup();
    }
}
