using UnityEngine;

[ExecuteAlways]
public class Route : BaseBehaviour
{
    [field: SerializeField, DescendantField]
    private Transform _points;
}
