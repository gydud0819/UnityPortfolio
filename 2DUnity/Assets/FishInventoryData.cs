using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FishInventoryData", menuName = "Game/Fish Inventory Data")]
public class FishInventoryData : ScriptableObject
{
    [System.Serializable]
    public class FishSlot
    {
        public FishType fishType;
        public Sprite fishIcon;
        public int count;
    }

    public List<FishSlot> caughtFishList = new List<FishSlot>();

    public void AddFish(FishType type, Sprite icon)
    {
        var existing = caughtFishList.Find(f => f.fishType == type);
        if (existing != null)
        {
            existing.count++;
        }
        else
        {
            caughtFishList.Add(new FishSlot
            {
                fishType = type,
                fishIcon = icon,
                count = 1
            });
        }
        Debug.Log($"[FishInventoryData] {type} 추가됨. 총 {caughtFishList.Count}종 보유 중.");
    }

    public void Clear()
    {
        caughtFishList.Clear();
        Debug.Log("[FishInventoryData] 전체 데이터 초기화 완료.");
    }
}
