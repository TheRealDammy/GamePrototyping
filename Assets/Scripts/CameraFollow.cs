using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private Vector3 shakeOffset;

    private void LateUpdate()
    {
        transform.position = target.position + offset + shakeOffset;
    }

    public void SetShakeOffset(Vector3 offset)
    {
        shakeOffset = offset;
    }
}
