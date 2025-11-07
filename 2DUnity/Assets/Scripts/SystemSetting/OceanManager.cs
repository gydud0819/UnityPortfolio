using System.Collections;
using UnityEngine;

public class OceanManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FishSpawner fishSpawner;
    [SerializeField] private OxygenManager oxygenManager;
    [SerializeField] private InventoryUI inventoryUI;

    // ✅ GameManager에서 직접 초기화해줄 때 호출됨
    public void Initialize(FishSpawner spawner, OxygenManager oxygen, InventoryUI inv)
    {
        fishSpawner = spawner;
        oxygenManager = oxygen;
        inventoryUI = inv;
        Debug.Log("[OceanManager] Initialize 완료 ✅");
    }

    private void Awake()
    {
        Debug.Log("[OceanManager] Awake 실행됨");
    }

    private void Start()
    {
        StartCoroutine(LateStart());
    }

    private IEnumerator LateStart()
    {
        yield return null;

        fishSpawner?.FishSpawn();
        oxygenManager?.ResetOxygen();
    }

    public void AddCaughtFish(string fishName)
    {
        if (inventoryUI == null)
        {
            Debug.LogError("[OceanManager] InventoryUI 연결 안 됨 ❌");
            return;
        }

        var info = FishDataLoader.Instance.GetFishInfo(fishName);
        if (info == null)
        {
            Debug.LogWarning($"[OceanManager] {fishName} 정보를 찾을 수 없습니다 ❌");
            return;
        }

        Sprite fishSprite = Resources.Load<Sprite>(info.worldSpritePath);
        if (fishSprite == null)
        {
            Debug.LogWarning($"[OceanManager] {info.worldSpritePath} 스프라이트 로드 실패 ❌");
            return;
        }

        if (System.Enum.TryParse(fishName, out FishType fishType))
        {
            inventoryUI.AddItemToUI(fishType, fishSprite);
            Debug.Log($"[OceanManager] {fishType} → 인벤토리 및 데이터 추가 완료 ✅");
        }
        else
        {
            Debug.LogWarning($"[OceanManager] {fishName} → FishType 변환 실패 ❌");
        }

        CaughtFishManager.AddFish(fishName);
    }
}
