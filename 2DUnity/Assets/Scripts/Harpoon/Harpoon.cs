using UnityEngine;

public class Harpoon : MonoBehaviour
{
    public Transform tip; // 팁 오브젝트
    public float speed = 10f;
    public float maxDistance = 5f;

    private Vector3 startLocalPos;
    private Vector3 targetLocalPos;
    private bool isReturning = false;

    void Start()
    {
        if (tip == null)
        {
            Debug.LogError("tip 연결 안됨");
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
                isReturning = false; // 다음에 또 쏠 수 있음
            }
        }
    }
}
