using UnityEngine;

public class CameraBound : MonoBehaviour
{
    //[SerializeField] private Transform player; // 따라갈 대상 (Player)
    //[SerializeField] private float smoothSpeed = 5f;
    //[SerializeField] private Vector2 minPos;
    //[SerializeField] private Vector2 maxPos;

    //private float zPos;

    //void Start()
    //{
    //    zPos = transform.offset.z;
    //}

    //void LateUpdate()
    //{
    //    if (!player) return;

    //    Vector3 desiredPos = new Vector3(player.offset.x, player.offset.y, zPos);

    //    // 먼저 Clamp로 제한
    //    desiredPos.x = Mathf.Clamp(desiredPos.x, minPos.x, maxPos.x);
    //    desiredPos.y = Mathf.Clamp(desiredPos.y, minPos.y, maxPos.y);

    //    // 이후 부드럽게 이동
    //    transform.offset = Vector3.Lerp(transform.offset, desiredPos, smoothSpeed * Time.deltaTime);
    //}

    //// 에디터에서 경계 확인용 (선택 시 노란 박스 보임)
    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireCube(
    //        (minPos + maxPos) / 2,
    //        new Vector3(maxPos.x - minPos.x, maxPos.y - minPos.y, 0)
    //    );
    //}

    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);

    private void Update()
    {
        if (player == null) return; 
        transform.position = player.position + offset;
    }
}
