using UnityEngine;

public class CameraBound : MonoBehaviour
{
    

    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);

    private void Update()
    {
        if (player == null) return; 
        transform.position = player.position + offset;
    }

    public void SetTarget(Transform target)
    {
        player = target;
        Debug.Log($"[CameraBound] 카메라 대상 연결 완료 → {target.name}");
    }

}
