using UnityEngine;

public class HarpoonTip : MonoBehaviour
{
    public Transform tip; // 작살 끝부분 오브젝트 (심)
    public float speed = 5f;
    public float maxDistance = 3f;

    private Vector3 startLocalPos;
    private bool isFiring = false;
    private bool isReturning = false;

    void Start()
    {
        startLocalPos = tip.localPosition;
    }

    public void Fire()
    {
        if (!isFiring)
        {
            isFiring = true;
            isReturning = false;
        }
    }

    void Update()
    {
        if (!isFiring) return;

        float step = speed * Time.deltaTime;

        if (!isReturning)
        {
            tip.localPosition = Vector3.MoveTowards(tip.localPosition, startLocalPos + Vector3.right * maxDistance, step);

            if (Vector3.Distance(tip.localPosition, startLocalPos + Vector3.right * maxDistance) < 0.01f)
            {
                isReturning = true;
            }
        }
        else
        {
            tip.localPosition = Vector3.MoveTowards(tip.localPosition, startLocalPos, step);

            if (Vector3.Distance(tip.localPosition, startLocalPos) < 0.01f)
            {
                isFiring = false;
            }
        }
    }
}
