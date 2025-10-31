using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Range(0.1f, 2f)]
    [SerializeField] private float parallaxFactor = 0.5f; // 1���� ������ ������, 1���� ũ�� ������

    private Transform cam;
    private Vector3 prevCamPos;

    private void Start()
    {
        cam = Camera.main.transform;
        prevCamPos = cam.position;
    }

    private void LateUpdate()
    {
        Vector3 delta = cam.position - prevCamPos;
        transform.position += delta * parallaxFactor;
        prevCamPos = cam.position;
    }
}
