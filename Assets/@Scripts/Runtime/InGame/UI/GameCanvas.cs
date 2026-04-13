using TMPro;
using UnityEngine;

public class GameCanvas : BaseBehaviour
{
    [SerializeField, DescendantField] private TextMeshProUGUI _waveText;
    [SerializeField, DescendantField] private TextMeshProUGUI _remainingTimeText;

    protected override void LateUpdate()
    {
        base.LateUpdate();

        var wave = Managers.WaveUpdater.Wave;
        _waveText.text = $"웨이브 : {wave}";

        var time = (int)Managers.WaveUpdater.RemainingTime;
        var m = time / 60;
        var s = time % 60;
        _remainingTimeText.text = $"{m:D2}:{s:D2}";
    }
}
