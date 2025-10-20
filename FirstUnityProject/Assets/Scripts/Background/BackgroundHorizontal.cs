using UnityEngine;

public class BackgroundHorizontal : MonoBehaviour
{
    public float scrollSpeed = 0.2f;
    private float width;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
        width = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float newPos = Mathf.Repeat(Camera.main.transform.position.x * scrollSpeed, width);

        transform.position = startPos + Vector3.left * newPos;
    }
}
