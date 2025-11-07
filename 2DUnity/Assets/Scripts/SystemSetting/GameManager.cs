using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject oceanMapPrefab;
    public GameObject fishSpawnerPrefab;
    public GameObject oxygenUIPrefab;
    public GameObject inventoryUIPrefab;
    public GameObject warningUIPrefab;
    public GameObject fishDataLoaderPrefab;

    private GameObject playerInstance;
    private GameObject oceanMapInstance;

    [Header("공용 인벤토리 데이터 (씬 공통 사용)")]
    [SerializeField] private FishInventoryData sharedInventoryData; // ✅ 바다·육지 공용

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        Debug.Log($"[GameManager] sharedInventoryData 해시: {sharedInventoryData?.GetHashCode()}");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(SetupSceneDelayed(scene));
    }

    private IEnumerator SetupSceneDelayed(Scene scene)
    {
        yield return null;

        if (scene.name == "Ocean")
            SetupOceanScene();
        else if (scene.name == "Land")
            SetupLandScene();
    }

    // 🌊 바다씬 세팅
    private void SetupOceanScene()
    {
        oceanMapInstance = Instantiate(oceanMapPrefab);
        playerInstance = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

        GameObject spawner = Instantiate(fishSpawnerPrefab);
        GameObject oxyUI = Instantiate(oxygenUIPrefab);
        GameObject invUI = Instantiate(inventoryUIPrefab);
        GameObject warnUI = Instantiate(warningUIPrefab);

        warnUI.transform.SetParent(null);
        warnUI.SetActive(false);

        // 🐟 데이터 로더 생성
        if (FindFirstObjectByType<FishDataLoader>() == null && fishDataLoaderPrefab != null)
        {
            Instantiate(fishDataLoaderPrefab);
            Debug.Log("[GameManager] FishDataLoader 인스턴스 생성 완료 ✅");
        }

        // 🌊 OceanManager 초기화
        OceanManager oceanManager = FindFirstObjectByType<OceanManager>();
        if (oceanManager != null)
        {
            oceanManager.Initialize(spawner.GetComponent<FishSpawner>(),
                                    oxyUI.GetComponent<OxygenManager>(),
                                    invUI.GetComponentInChildren<InventoryUI>(true));
        }

        // 🎒 InventoryUI 데이터 연결
        var invUIComp = invUI.GetComponentInChildren<InventoryUI>(true);
        if (invUIComp != null)
        {
            invUIComp.SetInventoryData(sharedInventoryData);
            Debug.Log("[GameManager] InventoryUI → sharedInventoryData 연결 완료 ✅");
        }

        // 🎥 카메라 추적 연결
        CameraBound cam = FindFirstObjectByType<CameraBound>();
        if (cam != null)
        {
            cam.GetType()
               .GetField("player", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
               ?.SetValue(cam, playerInstance.transform);
        }

        Debug.Log("[GameManager] Ocean 씬 초기화 완료 ✅");
    }

    // 🏝 육지씬 세팅
    private void SetupLandScene()
    {
        Debug.Log("[GameManager] 육지씬 로드됨");
        StartCoroutine(RefreshStorageUI());
    }

    private IEnumerator RefreshStorageUI()
    {
        yield return null;

        var storageUI = FindObjectOfType<StorageUI>();
        if (storageUI != null)
        {
            storageUI.SetInventoryData(sharedInventoryData); // ✅ 같은 데이터 연결
            storageUI.LoadFishData();
            Debug.Log("[GameManager] 보관함 UI 자동 갱신 완료");
        }
        else
        {
            Debug.LogWarning("[GameManager] StorageUI를 찾을 수 없음");
        }
    }

    // 🚪 바다로 이동
    public void GoToOcean() => SceneManager.LoadScene("Ocean");

    // 🏝 육지로 이동
    public void GoToLand() => SceneManager.LoadScene("Land");

    // 🫧 산소 0일 때 자동 귀환
    public void GoToFadeScene()
    {
        Debug.Log("[GameManager] 산소 0 → 페이드씬으로 전환 요청");

        var sceneryManager = FindObjectOfType<SceneryManager>();
        if (sceneryManager != null)
            sceneryManager.LoadScene("Land");
        else
            SceneManager.LoadScene("Land");
    }
}
