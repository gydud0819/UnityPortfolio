using UnityEngine;

public class Seaweed : MonoBehaviour
{

    [SerializeField] private float swayAmplitude = 0.5f; // Èçµé¸²ÀÇ ÁøÆø
    [SerializeField] private float swayFrequency = 1f;   // Èçµé¸²ÀÇ ºóµµ
    private Vector3 startScale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startScale = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float scaleY = 1f + Mathf.Sin(Time.time * swayAmplitude) * swayFrequency;
        transform.localScale = new Vector3(startScale.x, startScale.y * scaleY, startScale.z);
    }
}
