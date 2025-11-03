using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;

    [Header("Core References")]
    public PlayerCtrls player;
    public FishInventory fishInventory;
    public GameObject spriteMap;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
