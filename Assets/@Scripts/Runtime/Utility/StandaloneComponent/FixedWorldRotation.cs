using UnityEngine;

[ExecuteAlways]
public class FixedWorldRotation : MonoBehaviour
{
    public Vector3 worldRotation;

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(worldRotation);
    }
}
