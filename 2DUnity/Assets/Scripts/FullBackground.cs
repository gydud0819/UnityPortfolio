using UnityEngine;

public class FullBackground : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var sr = GetComponent<SpriteRenderer>();
        if (sr == null || sr.sprite == null) return;

        float screenH = Camera.main.orthographicSize * 2;
        float screenW = screenH * Screen.width / Screen.height;

        Vector2 spriteSize = sr.sprite.bounds.size;
        transform.localScale = new Vector3(screenW / spriteSize.x, screenH / spriteSize.y, 1);
    }

  
}
