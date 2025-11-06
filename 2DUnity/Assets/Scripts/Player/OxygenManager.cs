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
    private bool isSequenceRunning = false; // 추가: 중복 실행 방지
    private OxygenWarningUI warningUI;

    public void SetWarningUI(OxygenWarningUI ui)
    {
        warningUI = ui;
    }

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
        isSequenceRunning = true;
        Debug.Log("산소 부족 → 경고창 + 페이드아웃 시퀀스 실행");

        // 인벤토리 비활성화
        InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
        if (inventoryUI != null)
            inventoryUI.gameObject.SetActive(false);

        // 경고 UI 실행
        if (warningUI != null)
        {
            yield return StartCoroutine(warningUI.ShowWarningThenFadeOut());
        }
        else
        {
            Debug.LogWarning("⚠️ warningUI가 연결되지 않았습니다!");
        }

        // UI 다 끝난 뒤 비활성화
        yield return new WaitForSeconds(0.1f);
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
