using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class CaughtFishData
{
    public string fishName; // 잡은 물고기 이름
    public int count;       // 몇 마리 잡았는지
}

[System.Serializable]
public class CaughtFishList
{
    public List<CaughtFishData> caughtFishList = new List<CaughtFishData>();
}

public static class CaughtFishManager
{
    // 저장되는 실제 경로 (OS마다 다름, 유니티가 알아서 정해줌)
    private static string SavePath => Path.Combine(Application.persistentDataPath, "caught_fish.json");

    /// <summary>
    /// 저장 파일에서 데이터 불러오기
    /// </summary>
    public static CaughtFishList Load()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("[CaughtFishManager] 저장 파일 없음, 새로 생성할 예정");
            return new CaughtFishList();
        }

        try
        {
            string json = File.ReadAllText(SavePath);
            if (string.IsNullOrEmpty(json))
                return new CaughtFishList();

            return JsonUtility.FromJson<CaughtFishList>(json);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[CaughtFishManager] Load 실패: {e.Message}");
            return new CaughtFishList();
        }
    }

    /// <summary>
    /// 현재 데이터 저장
    /// </summary>
    public static void Save(CaughtFishList data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SavePath, json);
            Debug.Log($"[CaughtFishManager] 저장 완료: {SavePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[CaughtFishManager] Save 실패: {e.Message}");
        }
    }

    /// <summary>
    /// 물고기 한 종류 추가/카운트 증가
    /// </summary>
    public static void AddFish(string fishName)
    {
        if (string.IsNullOrEmpty(fishName))
        {
            Debug.LogWarning("[CaughtFishManager] 빈 이름 물고기 추가 시도됨");
            return;
        }

        // 기존 데이터 불러오기
        CaughtFishList data = Load();

        // 이미 잡은 물고기인지 확인
        CaughtFishData existing = data.caughtFishList.Find(f => f.fishName == fishName);
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

        // 저장
        Save(data);
        Debug.Log($"[CaughtFishManager] {fishName} 추가됨 (총 {data.caughtFishList.Count}종)");
    }

    /// <summary>
    /// 디버그용 전체 삭제 (원하면 나중에 버튼에 연결)
    /// </summary>
    public static void Clear()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("[CaughtFishManager] caught_fish.json 삭제됨");
        }
    }
}
