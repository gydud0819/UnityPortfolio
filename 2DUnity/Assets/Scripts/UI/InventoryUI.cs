using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{

    [SerializeField] private FishInventoryData fishInventoryData;
    [Header("슬롯 부모 오브젝트")]
    [SerializeField] private GameObject inventoryPanelRoot; // Inventory Bar 연결
    [SerializeField] private Transform quickbarParent;      // Q Content
    [SerializeField] private Transform inventoryParent;     // I Content

    [Header("슬롯 프리팹")]
    [SerializeField] private GameObject slotPrefab;

    [Header("슬롯 개수")]
    [SerializeField] private int quickbarSize = 9;
    [SerializeField] private int inventorySize = 27;

    private List<UISlot> quickbarSlots = new List<UISlot>();
    private List<UISlot> inventorySlots = new List<UISlot>();

    private PlayerCtrls playerCtrls;

    private bool isInventoryOpen = false;

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

    void Start()
    {
        // 인벤토리 패널 잠시 켜서 레이아웃 계산하게 하기
        inventoryPanelRoot.SetActive(true);

        CreateSlots(quickbarParent, quickbarSlots, quickbarSize);
        CreateSlots(inventoryParent, inventorySlots, inventorySize);

        // 다 만든 다음 전체 패널 비활성화
        inventoryPanelRoot.SetActive(false);
    }

    private void ToggleInventory()
    {
        Debug.Log("인벤토리 감지됨");
        isInventoryOpen = !isInventoryOpen;

        // 슬롯이 아니라 부모 패널(Inventory Bar)을 켜고 꺼야 함
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

        // 생성 후 강제 레이아웃 리빌드
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(parent.GetComponent<RectTransform>());
    }

    public void AddItemToUI(FishType fish, Sprite fishIcon)
    {
        if (fishIcon == null) return;

        // 퀵바에서 같은 물고기 찾기 (이름으로 비교)
        foreach (UISlot slot in quickbarSlots)
        {
            if (!slot.IsEmpty && slot.FishType == fish)
            {
                slot.AddCount();
                Debug.Log($"[InventoryUI] 동일 물고기({fishIcon.name}) → 퀵바 수량 증가");
                return;
            }
        }

        // 퀵바 빈 칸에 새로 추가
        foreach (UISlot slot in quickbarSlots)
        {
            if (slot.IsEmpty)
            {
                slot.SetItem(fishIcon, fish);
                Debug.Log($"[InventoryUI] {fishIcon.name} → 퀵바 새 슬롯 추가");
                return;
            }
        }

        // 인벤토리에서 같은 물고기 찾기
        foreach (UISlot slot in inventorySlots)
        {
            if (!slot.IsEmpty && slot.FishType == fish)
            {
                slot.AddCount();
                Debug.Log($"[InventoryUI] 동일 물고기({fishIcon.name}) → 인벤토리 수량 증가");
                return;
            }
        }

        // 인벤토리 빈 칸에 추가
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
