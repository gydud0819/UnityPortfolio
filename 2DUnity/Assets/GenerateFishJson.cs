using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

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

public class GenerateSimpleFishJson
{
    [MenuItem("Tools/Generate Simple Fish JSON")]
    public static void GenerateJson()
    {
        string fishPath = "Assets/Sprites/Fish";
        string savePath = "Assets/Resources/fish_data.json";

        string[] spriteGUIDs = AssetDatabase.FindAssets("t:Sprite", new[] { fishPath });
        Dictionary<string, string> addedFish = new Dictionary<string, string>();
        FishList fishList = new FishList();

        foreach (string guid in spriteGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string fileName = Path.GetFileNameWithoutExtension(path);

            // 이름에서 숫자 제거 (예: Blue_0 → Blue)
            string baseName = System.Text.RegularExpressions.Regex.Replace(fileName, @"_\d+$", "");

            // 중복 방지 — 같은 물고기는 첫 번째 프레임만 저장
            if (addedFish.ContainsKey(baseName)) continue;

            FishInfo info = new FishInfo
            {
                fishName = baseName,
                worldSpritePath = path.Replace("Assets/Resources/", "").Replace(".png", ""),
                description = "자동 생성된 물고기입니다."
            };
            fishList.fishList.Add(info);
            addedFish[baseName] = path;
        }

        string json = JsonUtility.ToJson(fishList, true);
        File.WriteAllText(savePath, json);
        AssetDatabase.Refresh();

        Debug.Log($" {fishList.fishList.Count}마리 물고기 데이터 생성 완료: {savePath}");
    }
}
