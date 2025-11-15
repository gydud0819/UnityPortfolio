using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneryManager : MonoBehaviour
{
    [SerializeField] GameObject screen;
    [SerializeField] Slider progress;
    [SerializeField] float displayProgress;

    public static SceneryManager Instance;

    private void Awake()
    {
        // 싱글톤 처리
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 시작 시에는 무조건 비활성화
        if (screen != null)
            screen.SetActive(false);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(TransitionScene(sceneName));
    }

    // SceneryManager.cs 내부

    public IEnumerator TransitionScene(string sceneName)
    {
        // 버튼 UI 비활성화 (Land 씬 기준)
        GameObject buttons = GameManager.Instance?.GetButtonCanvasInstance();
        if (buttons != null) buttons.SetActive(false);

        progress.value = 0;
        displayProgress = 0;
        screen.SetActive(true);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                progress.value = Mathf.Lerp(progress.value, 1.0f, Time.deltaTime);

                if (progress.value >= 0.99f)
                {
                    asyncOperation.allowSceneActivation = true;
                    screen.SetActive(false);

                    // ? 씬 전환 완료 후 버튼 UI 다시 활성화
                    if (sceneName == "Land" && buttons != null)
                        buttons.SetActive(true);

                    yield break;
                }
            }
            else
            {
                progress.value = asyncOperation.progress;
            }

            yield return null;
        }
    }

}
