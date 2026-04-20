using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : BaseBehaviour
{
    [SerializeField, DescendantField] private Button _startButton;
    [SerializeField, DescendantField] private Button _settingButton;

    protected override void OnInit()
    {
        base.OnInit();

        _startButton.onClick.AddListener(OnStart);
    }

    private void OnStart()
    {
        Managers.PopupManager.ShowPopup<TicketUI>("Prefabs/TicketUI");
    }
}
