using UnityEngine;

public class FishInventoryManager : MonoBehaviour
{
    [SerializeField] private InventoryUI inventoryUI;

    void Awake()
    {
        // 인스펙터 연결이 비어 있으면 자동으로 찾아줌
        if (inventoryUI == null)
        {
            inventoryUI = FindObjectOfType<InventoryUI>();
            Debug.Log($"[FishInventoryManager] Awake에서 InventoryUI 자동할당됨: {inventoryUI}");
        }
    }

    public void AddFish(FishType fish, Sprite fishSprite)
    {
        if(fishSprite == null)
        {
            Debug.Log("fish sprite is null. ui에 표시할 수 없음");
            return;
        }

        if(inventoryUI == null)
        {
            Debug.Log("inventoryUI가 연결되어 있지 않음");
            return;
        }

        Debug.Log($"[FishInventoryManager] 물고기가 인벤토리에 추가됨. {fishSprite.name}");

        inventoryUI.AddItemToUI(fish, fishSprite);
    }
}
