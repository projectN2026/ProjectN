using UnityEngine;

[ExecuteAlways]
public class MatchLocalPosition : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private void LateUpdate()
    {
        if (target == null)
            return;

        transform.localPosition = target.localPosition + offset;
    }
}
