using UnityEngine;
using UnityEngine.Tilemaps;

public class ParallaxLayer : MonoBehaviour
{
    public Transform cam;           // ���� ī�޶�
    public float parallax = 0.5f;   // ��� ������ �ӵ� (0~1)
    private float spriteWidth;      // �� ����� ���� ����
    private Vector3 startPos;

    void Start()
    {
        if (!cam) cam = Camera.main.transform;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        spriteWidth = sr.bounds.size.x;  // ��� 1���� ���� ��
        startPos = transform.position;
    }

    void LateUpdate()
    {
        float distance = (cam.position.x * parallax);
        transform.position = new Vector3(startPos.x + distance, startPos.y, startPos.z);

        // ī�޶� ��� �� �� �ʺ� �Ѿ�� ��ġ ���ġ
        float offset = cam.position.x * (1 - parallax);
        if (offset > startPos.x + spriteWidth)
            startPos.x += spriteWidth * 2f;
        else if (offset < startPos.x - spriteWidth)
            startPos.x -= spriteWidth * 2f;
    }
}
