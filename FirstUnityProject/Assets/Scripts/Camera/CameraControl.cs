using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;       // 따라갈 대상 (플레이어)
    public Vector3 offset;         // 위치 오프셋
    public float smoothSpeed = 0.125f; // 부드럽게 따라가는 정도

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
