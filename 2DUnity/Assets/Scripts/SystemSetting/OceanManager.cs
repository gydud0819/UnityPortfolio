using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FishSpawner fishSpawner;
    [SerializeField] private OxygenManager oxygenManager;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private OxygenWarningUI warningUI;

    private void Awake()
    {
        // 초기화 로그 찍기
        Debug.Log("[OceanManager] Awake 실행됨 - 내부 참조 준비");
    }

    void Start()
    {
        StartCoroutine(LateStart());
    }

    private IEnumerator LateStart()
    {
        yield return null;

        // 물고기 스폰 초기화
        if (fishSpawner != null)
            fishSpawner.FishSpawn();

        // 산소 게이지 리셋
        if (oxygenManager != null)
        {
            oxygenManager.ResetOxygen();
        }
    }

    public void AddCaughtFish(string fishName)
    {
        if (inventoryUI == null)
        {
            Debug.LogError("[OceanManager] InventoryUI가 연결되지 않음!");
            return;
        }

        var info = FishDataLoader.Instance.GetFishInfo(fishName);
        if (info == null)
        {
            Debug.LogWarning($"[OceanManager] {fishName} 정보를 찾을 수 없습니다.");
            return;
        }

        Sprite fishSprite = Resources.Load<Sprite>(info.worldSpritePath);
        if (fishSprite == null)
        {
            Debug.LogWarning($"[OceanManager] {info.worldSpritePath} 스프라이트 로드 실패");
            return;
        }

        if (System.Enum.TryParse(fishName, out FishType fishType))
        {
            inventoryUI.AddItemToUI(fishType, fishSprite);
            Debug.Log($"[OceanManager] {fishType} 인벤토리에 추가됨");
        }
        else
        {
            Debug.LogWarning($"[OceanManager] {fishName} → FishType 변환 실패");
        }

        CaughtFishManager.AddFish(fishName);
    }
}
