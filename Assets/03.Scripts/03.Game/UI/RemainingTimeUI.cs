using TMPro;
using UnityEngine;

public class RemainingTimeUI : BaseBehaviour
{
    [SerializeField, DescendantField]
    private TextMeshProUGUI _remainingTimeText;

    protected override void LateUpdate()
    {
        base.LateUpdate();

        var time = (int)Game.Instance.RemainingTime;
        var m = time / 60;
        var s = time % 60;

        _remainingTimeText.text = $"{m:D2}:{s:D2}";
    }
}
