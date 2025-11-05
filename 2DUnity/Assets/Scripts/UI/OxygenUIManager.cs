using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OxygenUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private Image fadeImage;

    public IEnumerator ShowWarningAndFadeOut()
    {
        // 1. "산소가 부족합니다" 표시
        warningText.gameObject.SetActive(true);
        countdownText.gameObject.SetActive(false);
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(0, 0, 0, 0);
        yield return new WaitForSeconds(1.2f);

        // 2. 카운트다운 시작
        warningText.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        // 3. 페이드아웃 효과
        float fadeDuration = 1.5f;
        float t = 0;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        // 4. 씬 전환
        SceneManager.LoadScene("LandScene");
    }
}
