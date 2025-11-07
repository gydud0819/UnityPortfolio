using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] public FishInventoryData sharedInventoryData;

    [Header("슬롯 부모 오브젝트")]
    [SerializeField] private GameObject inventoryPanelRoot; // Inventory Bar 연결
    [SerializeField] private Transform quickbarParent;      // Q Content
    [SerializeField] private Transform inventoryParent;     // I Content

    [Header("슬롯 프리팹")]
    [SerializeField] private GameObject slotPrefab;

    [Header("슬롯 개수")]
    [SerializeField] private int quickbarSize = 9;
    [SerializeField] private int inventorySize = 27;

    private List<UISlot> quickbarSlots = new();
    private List<UISlot> inventorySlots = new();

    private PlayerCtrls playerCtrls;
    private bool isInventoryOpen = false;

    // ? GameManager에서 공용 데이터 직접 주입할 수 있도록
    public void SetInventoryData(FishInventoryData data)
    {
        sharedInventoryData = data;
        Debug.Log($"[InventoryUI] sharedInventoryData 주입 완료 ({data.GetHashCode()})");
    }

    private void Awake()
    {
        playerCtrls = new PlayerCtrls();
        playerCtrls.Player.Inventory.performed += _ => ToggleInventory();
        playerCtrls.Enable();
    }

    private void OnDisable()
    {
        if (playerCtrls != null)
        {
            playerCtrls.Player.Inventory.performed -= _ => ToggleInventory();
            playerCtrls.Disable();
        }
    }

    private void Start()
    {
        inventoryPanelRoot.SetActive(true);

        CreateSlots(quickbarParent, quickbarSlots, quickbarSize);
        CreateSlots(inventoryParent, inventorySlots, inventorySize);

        inventoryPanelRoot.SetActive(false);
    }

    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryPanelRoot.SetActive(isInventoryOpen);
    }

    private void CreateSlots(Transform parent, List<UISlot> list, int size)
    {
        for (int i = 0; i < size; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, parent);
            UISlot slot = newSlot.GetComponent<UISlot>();
            list.Add(slot);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(parent.GetComponent<RectTransform>());
    }

    // ?? 물고기 추가
    public void AddItemToUI(FishType fish, Sprite fishIcon)
    {
        if (fishIcon == null) return;

        // 데이터에도 바로 반영
        sharedInventoryData?.AddFish(fish, fishIcon);

        // 퀵바 먼저 채우기
        foreach (UISlot slot in quickbarSlots)
        {
            if (!slot.IsEmpty && slot.FishType == fish)
            {
                slot.AddCount();
                Debug.Log($"[InventoryUI] {fish} → 퀵바 수량 증가");
                return;
            }
        }

        foreach (UISlot slot in quickbarSlots)
        {
            if (slot.IsEmpty)
            {
                slot.SetItem(fishIcon, fish);
                Debug.Log($"[InventoryUI] {fish} → 퀵바 새 슬롯 추가");
                return;
            }
        }

        // 인벤토리 슬롯
        foreach (UISlot slot in inventorySlots)
        {
            if (!slot.IsEmpty && slot.FishType == fish)
            {
                slot.AddCount();
                Debug.Log($"[InventoryUI] {fish} → 인벤토리 수량 증가");
                return;
            }
        }

        foreach (UISlot slot in inventorySlots)
        {
            if (slot.IsEmpty)
            {
                slot.SetItem(fishIcon, fish);
                Debug.Log($"[InventoryUI] {fish} → 인벤토리 새 슬롯 추가");
                return;
            }
        }

        Debug.LogWarning("인벤토리가 가득 찼습니다 ?");
    }
}
