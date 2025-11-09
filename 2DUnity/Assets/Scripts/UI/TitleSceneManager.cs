using UnityEngine;

public class TitleSceneManager : MonoBehaviour
{
    // Update is called once per frame

    private bool isLoading = false;
    void Update()
    {
        if (isLoading) return;

        if(Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            isLoading = true;
            // 터치 또는 클릭이 감지되면 게임 씬으로 전환
            UnityEngine.SceneManagement.SceneManager.LoadScene("Land");
        }
    }
}
