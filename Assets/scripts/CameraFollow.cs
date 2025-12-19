using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Camera Settings")]
    public Vector3 offset = new Vector3(8, 10, -8);
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.rotation = Quaternion.Euler(45, 45, 0);
    }
}