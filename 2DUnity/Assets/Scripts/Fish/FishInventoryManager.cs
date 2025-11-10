using UnityEngine;

public class FishInventoryManager : MonoBehaviour
{
    [SerializeField] private InventoryUI inventoryUI;

    void Awake()
    {
        // 인스펙터 연결이 비어 있으면 자동 검색
        if (inventoryUI == null)
        {
            inventoryUI = FindObjectOfType<InventoryUI>();
            Debug.Log($"[FishInventoryManager] Awake에서 InventoryUI 자동 연결 완료 ({inventoryUI?.name})");
        }
    }

    /// <summary>
    /// 바다에서 잡은 물고기를 인벤토리 및 데이터에 추가
    /// </summary>
    public void AddFish(FishType fish, Sprite fishSprite)
    {
        if (fish == null)
        {
            Debug.LogWarning("[FishInventoryManager] FishType이 비어 있음");
            return;
        }

        // ?? 스프라이트 보정 (없을 경우 Resources에서 로드)
        if (fishSprite == null)
        {
            string path = $"Sprites/Fish/{fish}";
            fishSprite = Resources.Load<Sprite>(path);

            if (fishSprite == null)
            {
                Debug.LogWarning($"[FishInventoryManager] {path} 스프라이트 로드 실패");
                return;
            }
        }

        // InventoryUI 연결 확인
        if (inventoryUI == null)
        {
            Debug.LogWarning("[FishInventoryManager] InventoryUI가 연결되지 않음");
            return;
        }

        // UI 업데이트
        inventoryUI.AddItemToUI(fish, fishSprite);
        Debug.Log($"[FishInventoryManager] UI에 {fish} 추가 완료");

        // ScriptableObject(sharedInventoryData)에 반영
        var gm = GameManager.Instance;
        if (gm != null)
        {
            var sharedData = gm.GetSharedInventoryData();
            if (sharedData != null)
            {
                sharedData.AddFish(fish, fishSprite);
                Debug.Log($"[FishInventoryManager] sharedInventoryData에 {fish} 저장 완료");
            }
            else
            {
                Debug.LogWarning("[FishInventoryManager] sharedInventoryData가 null임");
            }
        }
        else
        {
            Debug.LogWarning("[FishInventoryManager] GameManager.Instance 없음");
        }

        // JSON 백업 (optional)
        //CaughtFishManager.AddFish(fish.ToString());
    }
}
