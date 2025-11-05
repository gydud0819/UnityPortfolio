using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OxygenManager : MonoBehaviour
{
    [Header("산소 게이지 설정")]
    [SerializeField] private float maxOxygen = 99;
    [SerializeField] private float currentOxyen;

    [Header("UI 연결")]
    [SerializeField] private Slider oxygenSlider;

    private bool isActive = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentOxyen = maxOxygen;
        if (oxygenSlider != null)
        {
            oxygenSlider.maxValue = maxOxygen;
            oxygenSlider.value = currentOxyen;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;

        currentOxyen -= Time.deltaTime;
        if (oxygenSlider != null)
        {
            oxygenSlider.value = currentOxyen;
        }

        if (currentOxyen <= 0)
        {
            currentOxyen = 0;
            isActive = false;
            Debug.Log("산소가 부족합니다. 지상으로 귀환합니다.");

            SceneryManager sceneryManager = FindObjectOfType<SceneryManager>();
            if(sceneryManager != null)
            {
                sceneryManager.LoadScene(0); // 0은 지상 씬의 빌드 인덱스라고 가정
            }
            else
            {
                Debug.LogWarning("SceneryManager를 찾을 수 없음.");
            }
            
        }
    }

    void ReturnToLand()
    {
        SceneManager.LoadScene("Land");

    }

    public void ResetOxygen()
    {
        currentOxyen = maxOxygen;
        if (oxygenSlider != null)
        {
            oxygenSlider.value = currentOxyen;
        }
        isActive = true;
    }
}
