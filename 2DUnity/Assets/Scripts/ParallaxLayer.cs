using UnityEngine;
using UnityEngine.Tilemaps;

public class ParallaxLayer : MonoBehaviour
{
    public Transform cam;           // 메인 카메라
    public float parallax = 0.5f;   // 배경 움직임 속도 (0~1)
    private float spriteWidth;      // 한 배경의 가로 길이
    private Vector3 startPos;

    void Start()
    {
        if (!cam) cam = Camera.main.transform;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        spriteWidth = sr.bounds.size.x;  // 배경 1장의 실제 폭
        startPos = transform.position;
    }

    void LateUpdate()
    {
        float distance = (cam.position.x * parallax);
        transform.position = new Vector3(startPos.x + distance, startPos.y, startPos.z);

        // 카메라가 배경 한 장 너비를 넘어가면 위치 재배치
        float offset = cam.position.x * (1 - parallax);
        if (offset > startPos.x + spriteWidth)
            startPos.x += spriteWidth * 2f;
        else if (offset < startPos.x - spriteWidth)
            startPos.x -= spriteWidth * 2f;
    }
}
