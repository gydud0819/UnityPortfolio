using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("슬롯 부모 오브젝트")]
    [SerializeField] private Transform quickbarParent;
    [SerializeField] private Transform inventoryParent;

    [Header("슬롯 프리팹")]
    [SerializeField] private GameObject slotPrefab;

    [Header("슬롯 개수")]
    [SerializeField] private int quickbarSize = 9;
    [SerializeField] private int inventorySize = 27;

    private List<UISlot> quickbarSlots = new List<UISlot>();
    private List<UISlot> inventorySlots = new List<UISlot>();

    private PlayerCtrls playerCtrls;
    private bool isInventoryOpen = false;

    private void OnEnable() => playerCtrls.Enable();
    private void OnDisable() => playerCtrls.Disable();

    private void Awake()
    {
        playerCtrls = GetComponent<PlayerCtrls>();
        playerCtrls.Player.Inventory.performed += _ => ToggleInventory();
    }


    void Start()
    {
        CreateSlots(quickbarParent, quickbarSlots, quickbarSize);

        CreateSlots(inventoryParent, inventorySlots, inventorySize);

        inventoryParent.gameObject.SetActive(false);
    }

    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryParent.gameObject.SetActive(isInventoryOpen);
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

    public void AddItemToUI(FishType fish, Sprite fishIcon)
    {
        if (fishIcon == null) return;

        // 퀵바에서 같은 물고기 찾기 (이름으로 비교)
        foreach (UISlot slot in quickbarSlots)
        {
            if (!slot.IsEmpty && slot.FishType == fish)
            {
                slot.AddCount();   // ★ 수량만 증가
                Debug.Log($"[InventoryUI] 동일 물고기({fishIcon.name}) → 퀵바 수량 증가");
                return;
            }
        }

        // 퀵바 빈 칸에 새로 추가
        foreach (UISlot slot in quickbarSlots)
        {
            if (slot.IsEmpty)
            {
                slot.SetItem(fishIcon, fish);   // 새 아이템 세팅
                Debug.Log($"[InventoryUI] {fishIcon.name} → 퀵바 새 슬롯 추가");
                return;
            }
        }

        // 27칸 인벤토리용 
        foreach (UISlot slot in inventorySlots)
        {
            if (!slot.IsEmpty &&
                slot.CurrentSprite != null &&
                slot.CurrentSprite.name == fishIcon.name)
            {
                slot.AddCount();
                Debug.Log($"[InventoryUI] 동일 물고기({fishIcon.name}) → 인벤토리 수량 증가");
                return;
            }
        }

        foreach (UISlot slot in inventorySlots)
        {
            if (slot.IsEmpty)
            {
                slot.SetItem(fishIcon, fish);
                Debug.Log($"[InventoryUI] {fishIcon.name} → 인벤토리 새 슬롯 추가");
                return;
            }
        }

        Debug.Log("인벤토리가 가득 찼습니다.");
    }
}
