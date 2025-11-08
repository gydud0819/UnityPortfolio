using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OxygenManager : MonoBehaviour
{
    [Header("산소 게이지 설정")]
    [SerializeField] private float maxOxygen = 99;
    [SerializeField] private float currentOxygen;

    [Header("UI 연결")]
    [SerializeField] private Slider oxygenSlider;

    private bool isActive = true;
    private bool isSequenceRunning = false;

    void Start()
    {
        currentOxygen = maxOxygen;

        if (oxygenSlider != null)
        {
            oxygenSlider.maxValue = maxOxygen;
            oxygenSlider.value = currentOxygen;
        }
    }

    void Update()
    {
        if (!isActive || isSequenceRunning) return;

        currentOxygen -= Time.deltaTime;

        if (oxygenSlider != null)
            oxygenSlider.value = currentOxygen;

        if (currentOxygen <= 0)
        {
            currentOxygen = 0;
            isActive = false;

            if (!isSequenceRunning)
                StartCoroutine(OxygenDepletedSequence());
        }
    }

    private IEnumerator OxygenDepletedSequence()
    {
        if (isSequenceRunning) yield break;
        isSequenceRunning = true;

        Debug.Log("산소 부족 → 육지로 전환 준비");

        // 자연스러운 전환 텀
        yield return new WaitForSeconds(0.1f);

        // 인벤토리 삭제
        var inv = GameObject.Find("InventoryCanvas(Clone)");
        if (inv != null)
        {
            Destroy(inv);
            Debug.Log("[OxygenManager] 인벤토리 삭제 완료 ✅");
        }

        // 산소 UI 삭제
        var oxy = GameObject.Find("OxygenUI(Clone)");
        if (oxy != null)
        {
            Destroy(oxy);
            Debug.Log("[OxygenManager] 산소 UI 삭제 완료 ✅");
        }

        // 씬 이동
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GoToFadeScene();
        }
        else
        {
            Debug.LogWarning("[OxygenManager] GameManager.Instance 없음 → 직접 Land로 이동");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Land");
        }

        isSequenceRunning = false;
    }

    public void ResetOxygen()
    {
        currentOxygen = maxOxygen;
        if (oxygenSlider != null)
            oxygenSlider.value = currentOxygen;

        isActive = true;
        isSequenceRunning = false;
    }
}
