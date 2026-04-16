using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class RouteUI : BaseBehaviour
{
    [SerializeField, DescendantField] private UILineRenderer _backLine;
    [SerializeField, DescendantField] private UILineRenderer _fillLine;
    [SerializeField, DescendantsField] private RoutePointUI[] _points;

    [SerializeField, Range(0, 1)] 
    private float _fillProgress;

    public float EffectSec = 0.1f;
    public Color BackColor = Color.gray;
    public Color FillColor = Color.cyan;
    public int RouteNumber;
    public bool IsFill;

    protected override void Update()
    {
        base.Update();

        if (IsFill && _fillProgress < 1)
        {
            _fillProgress += Time.deltaTime / EffectSec;
            if (_fillProgress > 1)
                _fillProgress = 1;
        }
        if (!IsFill && _fillProgress > 0)
        {
            _fillProgress -= Time.deltaTime / EffectSec;
            if ( _fillProgress < 0)
                _fillProgress = 0;
        }

        UpdateBack();
        UpdateFill(_fillProgress);
    }

    private void UpdateBack()
    {
        if (_backLine == null) 
            return;

        var points = new List<Vector2>();

        foreach (var point in  _points)
        {
            var p = point.transform.localPosition;
            points.Add(p);
        }

        _backLine.color = BackColor;
        _backLine.Points = points;
    }
    private void UpdateFill(float progress)
    {
        if (_fillLine == null) return;
        if (_points == null) return;

        var points = new List<Vector2>();
        var lineProgress = progress * (_points.Length - 1);
        
        for (int i = 0; i < _points.Length; i++)
        {
            var point = _points[i];
            var position = point.transform.localPosition;

            point.SetColor(BackColor);

            if (lineProgress != 0 && i <= lineProgress)
            {
                point.SetColor(FillColor);
                points.Add(position);
            }
        }

        if (lineProgress > (int)lineProgress)
        {
            var a = _points[(int)lineProgress].transform.localPosition;
            var b = _points[(int)lineProgress + 1].transform.localPosition;
            var t = lineProgress - (int)lineProgress;
            var p = Vector2.Lerp(a, b, t);
            points.Add(p);
        }

        _fillLine.color = FillColor;
        _fillLine.Points = points;
    }
}
