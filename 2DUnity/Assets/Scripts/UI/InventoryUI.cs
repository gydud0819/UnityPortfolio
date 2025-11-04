using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("슬롯 부모 오브젝트")]
    [SerializeField] private Transform hotbarParent;
    [SerializeField] private Transform inventoryParent;

    [Header("슬롯 프리팹")]
    [SerializeField] private GameObject slotPrefab;

    [Header("슬롯 개수")]
    [SerializeField] private int hotbarSize = 9;
    [SerializeField] private int inventorySize = 27;

    private List<UISlot> hotbarSlots = new List<UISlot>();
    private List<UISlot> inventorySlots = new List<UISlot>();

    void Start()
    {
        //CreateSlots(hotbarParent, hotbarSlots, hotbarSize);
        //CreateSlots(inventoryParent, inventorySlots, inventorySize);
    }

    private void CreateSlots(Transform parent, List<UISlot> list, int size)
    {
        for (int i = 0; i < size; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, parent);
            UISlot slot = newSlot.GetComponent<UISlot>();
            list.Add(slot);
        }
    }

    public void AddItemToUI(Sprite fishIcon)
    {
        // 1. 먼저 같은 물고기가 이미 있는지 확인 (핫바)
        foreach (UISlot slot in hotbarSlots)
        {
            if (!slot.IsEmpty && slot.CurrentSprite == fishIcon)
            {
                slot.SetItem(fishIcon);
                Debug.Log($"[InventoryUI] 동일 물고기 발견 → 수량 증가");
                return;
            }
        }

        // 2. 핫바 비어있는 칸 찾기
        foreach (UISlot slot in hotbarSlots)
        {
            if (slot.IsEmpty)
            {
                slot.SetItem(fishIcon);
                Debug.Log($"[InventoryUI] {fishIcon.name} → 핫바 새 슬롯 추가");
                return;
            }
        }

        // 3. 27칸 인벤토리 확인 (스택 먼저)
        foreach (UISlot slot in inventorySlots)
        {
            if (!slot.IsEmpty && slot.CurrentSprite == fishIcon)
            {
                slot.SetItem(fishIcon);
                Debug.Log($"[InventoryUI] 동일 물고기 발견 → 인벤토리 수량 증가");
                return;
            }
        }

        // 4. 인벤토리에 빈 칸 있으면 새로 추가
        foreach (UISlot slot in inventorySlots)
        {
            if (slot.IsEmpty)
            {
                slot.SetItem(fishIcon);
                Debug.Log($"[InventoryUI] {fishIcon.name} → 인벤토리 새 슬롯 추가");
                return;
            }
        }

        Debug.Log("인벤토리가 가득 찼습니다.");
    }
}
