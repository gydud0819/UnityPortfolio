using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class CaughtFishData
{
    public string fishName;
    public int count;
}

[System.Serializable]
public class CaughtFishList
{
    public List<CaughtFishData> caughtFishList = new List<CaughtFishData>();
}

public static class CaughtFishManager
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "caught_fish.json");

    /// <summary>
    /// 저장 파일에서 데이터 불러오기
    /// </summary>
    public static CaughtFishList Load()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("[CaughtFishManager] 저장 파일 없음 → 새로 생성 예정");
            return new CaughtFishList();
        }

        try
        {
            string json = File.ReadAllText(SavePath);
            if (string.IsNullOrEmpty(json))
                return new CaughtFishList();

            var data = JsonUtility.FromJson<CaughtFishList>(json);
            Debug.Log($"[CaughtFishManager] JSON 로드 완료 ? ({data.caughtFishList.Count}종)");
            return data;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[CaughtFishManager] Load 실패: {e.Message}");
            return new CaughtFishList();
        }
    }

    /// <summary>
    /// JSON 파일로 데이터 저장
    /// </summary>
    public static void Save(CaughtFishList data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            using (FileStream fs = new FileStream(SavePath, FileMode.Create, FileAccess.Write))
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(json);
            }
            Debug.Log($"[CaughtFishManager] 저장 완료 ?? ({SavePath})");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[CaughtFishManager] Save 실패: {e.Message}");
        }
    }

    /// <summary>
    /// ScriptableObject에 JSON 데이터 반영 (예: 육지씬 진입 시)
    /// </summary>
    public static void SyncToInventoryData(FishInventoryData inventory)
    {
        if (inventory == null)
        {
            Debug.LogWarning("[CaughtFishManager] Sync 실패 - FishInventoryData 없음 ?");
            return;
        }

        var loaded = Load();
        inventory.Clear();

        foreach (var fish in loaded.caughtFishList)
        {
            if (System.Enum.TryParse(fish.fishName, out FishType fishType))
            {
                Sprite sprite = Resources.Load<Sprite>($"Sprites/Fish/{fish.fishName}");
                inventory.AddFish(fishType, sprite);
            }
        }

        Debug.Log($"[CaughtFishManager] JSON → ScriptableObject 동기화 완료 ? ({inventory.caughtFishList.Count}종)");
    }

    /// <summary>
    /// 물고기 한 종류 추가 및 저장
    /// </summary>
    public static void AddFish(string fishName)
    {
        if (string.IsNullOrEmpty(fishName))
        {
            Debug.LogWarning("[CaughtFishManager] 빈 이름 물고기 추가 시도됨 ?");
            return;
        }

        // 기존 JSON 데이터 불러오기
        CaughtFishList data = Load();

        // 이미 존재하는 물고기인지 확인
        var existing = data.caughtFishList.Find(f => f.fishName == fishName);
        if (existing != null)
        {
            existing.count++;
        }
        else
        {
            data.caughtFishList.Add(new CaughtFishData
            {
                fishName = fishName,
                count = 1
            });
        }

        // JSON 저장
        Save(data);

        // ? ScriptableObject에도 즉시 반영
        if (GameManager.Instance != null)
        {
            var sharedData = GameManager.Instance.GetSharedInventoryData();
            if (sharedData != null && System.Enum.TryParse(fishName, out FishType fishType))
            {
                Sprite icon = Resources.Load<Sprite>($"Sprites/Fish/{fishName}");
                sharedData.AddFish(fishType, icon);
                Debug.Log($"[CaughtFishManager] {fishName} → sharedInventoryData에도 반영 완료 ?");
            }
        }

        Debug.Log($"[CaughtFishManager] {fishName} 추가됨 (총 {data.caughtFishList.Count}종)");
    }

    /// <summary>
    /// 전체 초기화 (디버그용)
    /// </summary>
    public static void Clear()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("[CaughtFishManager] caught_fish.json 삭제됨 ??");
        }
    }
}
