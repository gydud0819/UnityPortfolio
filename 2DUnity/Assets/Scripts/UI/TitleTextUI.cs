using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleTextUI : MonoBehaviour
{
    [SerializeField] private Image targetImage; // Touch to Start 이미지
    [SerializeField] private float blinkSpeed = 1.0f;

    void Start()
    {
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while (true)
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * blinkSpeed;
                float alpha = Mathf.PingPong(t, 1f); // 0~1 반복
                Color color = targetImage.color;
                color.a = alpha;
                targetImage.color = color;
                yield return null;
            }
        }

        //while (true)
        //{
        //    targetImage.gameObject.SetActive(false);
        //    yield return new WaitForSeconds(0.5f);
        //    targetImage.gameObject.SetActive(true);
        //    yield return new WaitForSeconds(0.5f);
        //}
    }
}
