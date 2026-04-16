using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RouteSelectButtonUI : BaseBehaviour
{
    [SerializeField, ComponentField] private Button _button;
    [SerializeField, DescendantField] private TextMeshProUGUI _text;

    public int RouteNumber;

    protected override void OnInit()
    {
        base.OnInit();

        _button.onClick.AddListener(OnClick);
    }
    protected override void Update()
    {
        base.Update();

        _text.text = $"{RouteNumber}호선";

        if (Managers.GameDataManager.RouteNumber == RouteNumber)
            _button.interactable = false;
        else 
            _button.interactable = true;
    }

    private void OnClick()
    {
        Managers.GameDataManager.RouteNumber = RouteNumber;
    }
}
