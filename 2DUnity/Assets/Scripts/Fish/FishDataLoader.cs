using System.Collections.Generic;
using UnityEngine;

public class FishDataLoader : MonoBehaviour
{
    public static FishDataLoader Instance { get; private set; }

    [System.Serializable]
    public class FishInfo
    {
        public string fishName;
        public string worldSpritePath;
        public string description;
    }

    [System.Serializable]
    public class FishList
    {
        public List<FishInfo> fishList = new List<FishInfo>();
    }

    private FishList fishList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadFishData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// fish_data.json을 Resources 폴더에서 불러옴
    /// </summary>
    private void LoadFishData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("fish_data");
        if (jsonFile == null)
        {
            Debug.LogError("[FishDataLoader] fish_data.json을 Resources 폴더에서 찾을 수 없습니다!");
            return;
        }

        fishList = JsonUtility.FromJson<FishList>(jsonFile.text);
        Debug.Log($"[FishDataLoader] 물고기 데이터 {fishList.fishList.Count}개 로드 완료");
    }

    /// <summary>
    /// 이름으로 물고기 데이터 검색
    /// </summary>
    public FishInfo GetFishInfo(string fishName)
    {
        if (fishList == null || fishList.fishList.Count == 0)
        {
            Debug.LogWarning("[FishDataLoader] 데이터가 로드되지 않았습니다.");
            return null;
        }

        // 띄어쓰기 제거해서 비교 (Blue 1 == Blue1 도 동일 취급)
        string normalized = fishName.Replace(" ", "");
        foreach (var f in fishList.fishList)
        {
            if (f.fishName.Replace(" ", "") == normalized)
                return f;
        }

        Debug.LogWarning($"[FishDataLoader] {fishName} 정보를 찾을 수 없습니다.");
        return null;
    }

}
