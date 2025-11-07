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
            Sprite sprite = icon;
            if (sprite == null)
            {
                string path = $"Fish/{type}";
                Sprite[] loadedSprites = Resources.LoadAll<Sprite>(path);
                if (loadedSprites.Length > 0)
                {
                    sprite = loadedSprites[0];
                    Debug.Log($"[FishInventoryData] {path} 시트에서 첫 스프라이트 로드 성공 ?");
                }
                else
                {
                    sprite = Resources.Load<Sprite>(path);
                    if (sprite == null)
                        Debug.LogWarning($"[FishInventoryData] {path} 로드 실패 ?");
                    else
                        Debug.Log($"[FishInventoryData] {path} 단일 스프라이트 로드 성공 ?");
                }
            }

            // ? 여기서 리스트에 실제로 추가해야 함!
            caughtFishList.Add(new FishSlot
            {
                fishType = type,
                fishIcon = sprite,
                count = 1
            });

            Debug.Log($"[FishInventoryData] {type} 신규 추가 완료 ?");
        }

        Debug.Log($"[FishInventoryData] {type} 추가됨. 총 {caughtFishList.Count}종 보유 중.");
    }

    public void Clear()
    {
        caughtFishList.Clear();
        Debug.Log("[FishInventoryData] 전체 데이터 초기화 완료.");
    }

    public void TransferTo(FishInventoryData targetInventory)
    {
        foreach (var fish in caughtFishList)
        {
            var existing = targetInventory.caughtFishList.Find(f => f.fishType == fish.fishType);
            if (existing != null)
            {
                existing.count += fish.count;
            }
            else
            {
                // 여기서도 icon이 null일 수 있으니 다시 로드하도록 추가
                Sprite sprite = fish.fishIcon;
                if (sprite == null)
                {
                    string path = $"Sprites/Fish/{fish.fishType}";
                    sprite = Resources.Load<Sprite>(path);
                    Debug.Log($"[FishInventoryData] {fish.fishType} 전송 중 아이콘 재로드됨");
                }

                targetInventory.caughtFishList.Add(new FishSlot
                {
                    fishType = fish.fishType,
                    fishIcon = sprite,
                    count = fish.count
                });
            }
        }

        Debug.Log($"[FishInventoryData] {targetInventory.name}으로 {caughtFishList.Count}종 이동 완료 ?");
        Clear();
    }
}
