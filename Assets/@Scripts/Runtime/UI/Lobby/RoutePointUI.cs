using UnityEngine;
using UnityEngine.UI;

[InjectionByEditor]
[ExecuteAlways]
public class RoutePointUI : MonoBehaviour
{
    [SerializeField, ComponentField]
    private Image _image;

    [SerializeField]
    private Vector2 _offset;

    public int Index => transform.GetSiblingIndex();
    public int SiblingCount => transform.parent?.childCount ?? 0;
    public Transform PrevTransform
    {
        get
        {
            if (Index == 0) return null;
            else return transform.parent?.GetChild(Index - 1);
        }
    }
    public Transform NextTransform
    {
        get
        {
            if (Index == SiblingCount - 1) return null;
            else return transform.parent?.GetChild(Index + 1);
        }
    }
    public RoutePointUI Prev => PrevTransform?.GetComponent<RoutePointUI>();
    public RoutePointUI Next => NextTransform?.GetChild(Index - 1)?.GetComponent<RoutePointUI>();

    private void Update()
    {
        if (PrevTransform == null)
            transform.localPosition = _offset;
        else
            transform.localPosition = PrevTransform.localPosition + (Vector3)_offset;
    }

    public void SetColor(Color color)
    {
        _image.color = color;
    }
}
