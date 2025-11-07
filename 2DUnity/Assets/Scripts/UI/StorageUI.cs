using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageUI : MonoBehaviour
{
    [SerializeField] private FishInventoryData sharedInventoryData; // ? 공용 데이터
    [SerializeField] private Transform inventoryParent;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private List<FishType> slotFishOrder;
    [SerializeField] private int totalSlots = 27;

    private Dictionary<FishType, UISlot> slotMap = new();
    private List<UISlot> allSlots = new();

    public void SetInventoryData(FishInventoryData data)
    {
        sharedInventoryData = data;
        Debug.Log($"[StorageUI] sharedInventoryData 주입 완료 ({data.GetHashCode()})");
    }

    private void Awake()
    {
        CreateSlots();
        LoadFishData();
    }

    private void CreateSlots()
    {
        for (int i = 0; i < totalSlots; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, inventoryParent);
            UISlot slot = newSlot.GetComponent<UISlot>();
            allSlots.Add(slot);
        }

        for (int i = 0; i < slotFishOrder.Count && i < allSlots.Count; i++)
        {
            slotMap[slotFishOrder[i]] = allSlots[i];
            Debug.Log($"[StorageUI] {slotFishOrder[i]} → {i + 1}번 슬롯 매핑 완료 ?");
        }

        Debug.Log($"[StorageUI] 총 {slotMap.Count}/{totalSlots} 슬롯 매핑 완료");
    }

    public void LoadFishData()
    {
        if (sharedInventoryData == null)
        {
            Debug.LogWarning("[StorageUI] sharedInventoryData가 비어 있음 ?");
            return;
        }

        Debug.Log($"[StorageUI] 불러오기: {sharedInventoryData.caughtFishList.Count}마리");

        foreach (var fish in sharedInventoryData.caughtFishList)
        {
            Sprite icon = fish.fishIcon;
            if (icon == null)
            {
                string path = $"Fish/{fish.fishType}";
                Sprite[] sprites = Resources.LoadAll<Sprite>(path);
                if (sprites.Length > 0)
                    icon = sprites[0];
            }

            if (slotMap.TryGetValue(fish.fishType, out UISlot slot))
            {
                slot.SetItem(icon, fish.fishType);
                Debug.Log($"[StorageUI] {fish.fishType} → 도감형 슬롯 표시 ?");
            }
            else
            {
                UISlot emptySlot = allSlots.Find(s => s.IsEmpty && !slotMap.ContainsValue(s));
                if (emptySlot != null)
                {
                    emptySlot.SetItem(icon, fish.fishType);
                    Debug.Log($"[StorageUI] {fish.fishType} → 여분 슬롯 표시 ?");
                }
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(inventoryParent.GetComponent<RectTransform>());
    }
}
