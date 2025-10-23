using UnityEngine;

public class Harpoon : MonoBehaviour
{
    public Transform tip; // �� ������Ʈ
    public float speed = 10f;
    public float maxDistance = 5f;

    private Vector3 startLocalPos;
    private Vector3 targetLocalPos;
    private bool isReturning = false;

    void Start()
    {
        if (tip == null)
        {
            Debug.LogError("tip ���� �ȵ�");
            return;
        }

        startLocalPos = tip.localPosition;
        targetLocalPos = startLocalPos + Vector3.right * maxDistance;
    }

    void Update()
    {
        if (!isReturning)
        {
            tip.localPosition = Vector3.MoveTowards(tip.localPosition, targetLocalPos, speed * Time.deltaTime);

            if (Vector3.Distance(tip.localPosition, targetLocalPos) < 0.01f)
            {
                isReturning = true;
            }
        }
        else
        {
            tip.localPosition = Vector3.MoveTowards(tip.localPosition, startLocalPos, speed * Time.deltaTime);

            if (Vector3.Distance(tip.localPosition, startLocalPos) < 0.01f)
            {
                isReturning = false; // ������ �� �� �� ����
            }
        }
    }
}
