using UnityEngine;

public class Harpoon : MonoBehaviour
{
    [SerializeField] private Transform followTarget; // ���� �÷��̾�
    [SerializeField] private Vector3 offset;         // �� ��ġ�� �°� ������ ����
    [SerializeField] private bool followRotation = true;

    void Update()
    {
        if (followTarget == null) return;

        // ��ġ ���󰡱�
        transform.position = followTarget.position + offset;

        // ȸ���� ���󰡰� (�÷��̾ Z�� ȸ�� ���� ��)
        if (followRotation)
        {
            transform.rotation = followTarget.rotation;
        }
    }

    public void SetFollowTarget(Transform target, Vector3 customOffset)
    {
        followTarget = target;
        offset = customOffset;
    }
}
