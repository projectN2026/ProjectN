using UnityEngine;
using UnityEngine.UI;

public class StartButton : BaseBehaviour
{
    [field: SerializeField, ComponentField]
    private Button _button;

    protected override void OnInit()
    {
        base.OnInit();

        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        SceneLoader.Load(SceneType.GameScene);
    }
}
