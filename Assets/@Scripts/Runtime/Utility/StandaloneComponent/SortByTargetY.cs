using UnityEngine;

[ExecuteAlways]
public class SortByTargetY : MonoBehaviour
{
    [SerializeField]
    private Renderer _renderer;
    [SerializeField]
    private Transform _target;

    private void LateUpdate()
    {
        if (_renderer == null) return;
        if (_target == null) return;

        _renderer.sortingOrder = (int)(-_target.position.y * 100);
    }
}
