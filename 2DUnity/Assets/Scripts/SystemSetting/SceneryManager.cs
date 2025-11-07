using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneryManager : MonoBehaviour
{
    [SerializeField] GameObject screen;
    [SerializeField] Slider progress;
    [SerializeField] float displayProgress;
    private void Awake()
    {
        var objects = FindObjectsOfType<SceneryManager>();

        if (objects.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);      // 게임 오브젝트를 삭제하지 않고 같이 들고간다는 의미 

    }

    /// <summary>
    /// <AsyncOperation>
    /// allowSceneActivation : 장면이 준비된 즉시, 장면이 활성화되는 것을 허용하는 변수
    /// isDone : 해당 동작이 완료 되었는지 나타내는 변수 (읽기 전용)
    /// progress : 작업의 진행상태를 나타내는 변수 (읽기 전용)
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    /// 


    public void LoadScene(string sceneName)
    {
        StartCoroutine(TransitionScene(sceneName));
    }



    public IEnumerator TransitionScene(string sceneName)
    {
        progress.value = 0;     // 초기 세팅
        displayProgress = 0;
        screen.SetActive(true);
        // <AsyncOperation>
        // allowSceneActivation : 장면이 준비된 즉시, 장면이 활성화되는 것을 허용하는 변수

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;        // 장면 비활성화 

        // <AsyncOperation>
        // isDone : 해당 동작이 완료 되었는지 나타내는 변수 (읽기 전용)
        while (asyncOperation.isDone == false)
        {
            // <AsyncOperation>
            // progress : 작업의 진행상태를 나타내는 변수 (읽기 전용)
            if (asyncOperation.progress >= 0.9f)
            {
                progress.value = Mathf.Lerp(progress.value, 1.0f, Time.deltaTime);

                if (progress.value >= 0.99f)
                {
                    asyncOperation.allowSceneActivation = true;
                    screen.SetActive(false);
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
