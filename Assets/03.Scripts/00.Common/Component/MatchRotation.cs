using UnityEngine;

[ExecuteAlways]
public class MatchRotation : MonoBehaviour
{
    public Transform target;

    private void LateUpdate()
    {
        if (target == null)
            return;

        transform.rotation = target.rotation;
    }
}
