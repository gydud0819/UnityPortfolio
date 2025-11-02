using UnityEngine;

public class CameraBound : MonoBehaviour
{
    [SerializeField] private Transform target; // 따라갈 대상 (Player)
    [SerializeField] private Vector2 minPos;
    [SerializeField] private Vector2 maxPos;

    private float zPos;

    void Start()
    {
        zPos = transform.position.z;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 플레이어 좌표를 기준으로 카메라 제한
        Vector3 newPos = target.position;

        // 혹시 모를 튀는 현상 방지용 Clamp 범위 넉넉히
        newPos.x = Mathf.Clamp(newPos.x, minPos.x, maxPos.x);
        newPos.y = Mathf.Clamp(newPos.y, minPos.y, maxPos.y);

        transform.position = new Vector3(newPos.x, newPos.y, zPos);
    }

    // 유니티 인스펙터에서 테스트용으로 보기 쉽게
    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireCube(
    //        (minPos + maxPos) / 2,
    //        new Vector3(maxPos.x - minPos.x, maxPos.y - minPos.y, 0)
    //    );
    //}
}
