using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class OxygenWarningUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float displayDuration = 3f;

    public IEnumerator ShowWarningThenFadeOut()
    {
        if (canvasGroup == null || warningText == null)
        {
            Debug.LogWarning("[OxygenWarningUI] 연결 안 됨!");
            yield break;
        }

        // 초기화
        canvasGroup.alpha = 0;
        gameObject.SetActive(true);

        // 🔹 페이드인
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }

        // 경고 문구 표시 유지
        yield return new WaitForSeconds(displayDuration);

        // 페이드아웃
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }

        // 완전히 꺼진 후 비활성화
        canvasGroup.alpha = 0;
        gameObject.SetActive(false);

        // 페이드씬 전환 (이제 완전히 끝난 뒤 호출됨)
        yield return new WaitForSeconds(0.3f); // 약간의 여유
        if (GameManager.Instance != null)
        {
            Debug.Log("[OxygenWarningUI] 페이드아웃 완료 → Land 씬 전환 실행");
            GameManager.Instance.GoToFadeScene();
        }
        else
        {
            Debug.LogWarning("[OxygenWarningUI] GameManager.Instance 없음!");
        }
    }
}
