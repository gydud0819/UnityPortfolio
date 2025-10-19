using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;       // ���� ��� (�÷��̾�)
    public Vector3 offset;         // ��ġ ������
    public float smoothSpeed = 0.125f; // �ε巴�� ���󰡴� ����

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
