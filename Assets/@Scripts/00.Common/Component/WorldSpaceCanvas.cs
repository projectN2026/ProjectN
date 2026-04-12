using UnityEngine;

public class WorldSpaceCanvas : MonoBehaviour
{
    private Canvas _canvas;
    private Camera _camera;

    private void Start()
    {
        _canvas = GetComponent<Canvas>();
        _camera = _canvas.worldCamera ?? (_canvas.worldCamera = Camera.main);
    }

    private void LateUpdate()
    {
        if (_canvas == null) return;
        if (_camera == null) return;

        _canvas.transform.rotation = _camera.transform.rotation;
    }
}
