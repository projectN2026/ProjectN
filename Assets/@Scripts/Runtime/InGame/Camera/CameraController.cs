using UnityEngine;

public class CameraController : BaseBehaviour
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        var target = Managers.ObjectManager.Player;
        if (target == null)
            return;

        var targetPosition = target.transform.position;
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
    }
}
