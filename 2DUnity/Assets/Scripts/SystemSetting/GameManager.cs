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
    public GameObject fishDataLoaderPrefab; // 🟡 새로 추가됨!

    private GameObject playerInstance;
    private GameObject oceanMapInstance;

    void Awake()
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Ocean")
        {
            SetupOceanScene();
        }
        else if (scene.name == "Land")
        {
            SetupLandScene();
        }
    }

    private void SetupOceanScene()
    {
        // 🌊 Ocean씬 초기화
        oceanMapInstance = Instantiate(oceanMapPrefab);
        playerInstance = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        GameObject spawner = Instantiate(fishSpawnerPrefab);
        GameObject oxyUI = Instantiate(oxygenUIPrefab);
        GameObject invUI = Instantiate(inventoryUIPrefab);
        GameObject warnUI = Instantiate(warningUIPrefab);
        warnUI.SetActive(false); // 경고창은 기본 비활성화

        // 🐟 FishDataLoader 없으면 자동 생성
        if (FindFirstObjectByType<FishDataLoader>() == null && fishDataLoaderPrefab != null)
        {
            Instantiate(fishDataLoaderPrefab);
            Debug.Log("[GameManager] FishDataLoader 인스턴스 생성 완료 ✅");
        }

        // 🧭 OceanManager 연결
        OceanManager oceanManager = Object.FindFirstObjectByType<OceanManager>();
        if (oceanManager != null)
        {
            var type = oceanManager.GetType();

            type.GetField("fishSpawner", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(oceanManager, spawner.GetComponent<FishSpawner>());

            type.GetField("oxygenManager", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(oceanManager, oxyUI.GetComponent<OxygenManager>());

            // 🎒 인벤토리 UI는 자식에서 찾아야 함
            var invUIComp = invUI.GetComponentInChildren<InventoryUI>(true);
            if (invUIComp == null)
                Debug.LogError("[GameManager] InventoryUI를 InventoryCanvas에서 찾지 못했습니다!");
            else
                Debug.Log("[GameManager] InventoryUI 연결 성공");

            type.GetField("inventoryUI", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(oceanManager, invUIComp);

            type.GetField("warningUI", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(oceanManager, warnUI.GetComponent<OxygenWarningUI>());

            Debug.Log("[GameManager] OceanManager에 프리팹 참조 전달 완료 ✅");
        }
        else
        {
            Debug.LogWarning("[GameManager] OceanManager를 찾을 수 없습니다 ❌");
        }

        // 📸 카메라 플레이어 추적
        CameraBound cam = FindFirstObjectByType<CameraBound>();
        if (cam != null)
        {
            cam.GetType()
               .GetField("player", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
               .SetValue(cam, playerInstance.transform);
        }

        Debug.Log("[GameManager] Ocean 씬 초기화 완료 🌊");
    }

    private void SetupLandScene()
    {
        Debug.Log("[GameManager] 육지씬 로드됨");
    }

    // 🌊 바다로 이동
    public void GoToOcean()
    {
        SceneManager.LoadScene("Ocean");
    }

    // 🟩 육지로 바로 이동 (페이드 없이)
    public void GoToLand()
    {
        SceneManager.LoadScene("Land");
    }

    // 🟥 산소 0일 때 페이드씬 전환
    public void GoToFadeScene()
    {
        Debug.Log("[GameManager] 산소 0 → 페이드씬으로 전환 요청");

        SceneryManager sceneryManager = FindObjectOfType<SceneryManager>();
        if (sceneryManager != null)
        {
            int landSceneIndex = SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/Land.unity");
            if (landSceneIndex >= 0)
            {
                Debug.Log("[GameManager] 페이드씬 실행 → 육지로 이동 준비");
                sceneryManager.LoadScene(landSceneIndex);
            }
            else
            {
                Debug.LogWarning("[GameManager] Land 씬 인덱스 찾을 수 없음! 빌드 세팅 확인 필요");
            }
        }
        else
        {
            Debug.LogWarning("[GameManager] SceneryManager가 없어 바로 Land로 이동합니다.");
            SceneManager.LoadScene("Land");
        }
    }
}
