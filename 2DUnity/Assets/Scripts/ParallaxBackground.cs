using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Transform cam;           // 카메라 (Main Camera)
    [SerializeField] private float parallaxFactor = 0.5f; // 패럴랙스 깊이
    private Vector2 lastCamPos;
    private Vector2 startPos;

    void Start()
    {
        if (!cam) cam = Camera.main.transform;
        startPos = transform.position;
        lastCamPos = cam.position;
    }

    void LateUpdate()
    {
        // 카메라 이동량 계산
        Vector2 camDelta = (Vector2)cam.position - lastCamPos;
        lastCamPos = cam.position;

        // 배경을 카메라 이동 방향 반대로 패럴랙스 비율만큼 이동
        transform.position += (Vector3)(camDelta * parallaxFactor);
    }
}
