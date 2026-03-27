using UnityEngine;

public class MatchCameraRotation : MonoBehaviour
{
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        if (_camera == null)
            return;

        transform.rotation = _camera.transform.rotation;
    }
}
