using UnityEngine;

public class CameraController : BaseBehaviour
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        var target = Game.Instance.Object.transform;

        transform.position = Vector3.Lerp(transform.position, target.position, 0.1f);
    }
}
