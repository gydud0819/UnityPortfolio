using System.Collections.Generic;
using UnityEngine;

public class FishInventory : MonoBehaviour
{
    public static FishInventory Instance;
    private Dictionary<FishData, int> caughtFishDict = new Dictionary<FishData, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddFish(FishData fish)
    {
        if (caughtFishDict.ContainsKey(fish))
        {
            caughtFishDict[fish]++;
        }
        else
        {
            caughtFishDict[fish] = 1;
        }

        Debug.Log($"{fish.fishName} 잡음. 현재 수량: {caughtFishDict[fish]}");
    }

    public int GetFishCount(FishData fish)
    {
        if (caughtFishDict.TryGetValue(fish, out int count))
        {
            return count;
        }
        return 0;
    }

    public Dictionary<FishData, int> GetAllFish()
    {
        return caughtFishDict;
    }
}
