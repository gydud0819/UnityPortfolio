using UnityEngine;

public class HarpoonTip : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float maxDistance = 5f;

    private Vector3 startPos;
    private Vector3 direction;
    private bool isFlying = false;

    void Update()
    {
        if(!isFlying) return;

        transform.position += direction * speed * Time.deltaTime;
        if(Vector3.Distance(startPos, transform.position) >= maxDistance)
        {
            isFlying = false;
            gameObject.SetActive(false);
        }
    }

    private void Fire(Vector3 dir)
    {
        startPos = transform.position;
        direction = dir.normalized;
        isFlying = true;
        gameObject.SetActive(true);
    }
}
