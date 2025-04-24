// GameManager.cs - 게임 내 자금 및 상태 관리

using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Runtime.ConstrainedExecution;

public class GameManager : MonoBehaviour
{
    private int money;
    private const int moneyExchange = 10;
    public int Money => money;
    public static event Action<int> OnMoneyChanged;

    private const string MONEY_KEY = "Money";

    [Header("인게임 차량 프리팹")]
    public GameObject[] ingameCarPrefabs;
    //public Transform carSpawnPoint;  // 씬에 배치한 스폰 포인트

    public GameObject currentCar;
    public CarStats selectedStats;     // 플레이어가 선택한 SO

    private void Awake()
    {
        if (Shared.GameManager == null)
        {
            Shared.GameManager = this;
            DontDestroyOnLoad(gameObject);
            LoadMoney();

            // 씬 로드 콜백 구독
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //'eSCENE_INGAME' 씬 또는 'eSCENE_PRACTICE'일 때만
        if (scene.name == eSCENE.eSCENE_INGAME.ToString() || scene.name == eSCENE.eSCENE_PRACTICE.ToString())
        {
            SpawnSelectedCar();
            Debug.Log("자동차 생성됨 on " + scene.name);
        }
    }
    private void OnDestroy()
    {
        // 안전하게 이벤트 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void SpawnSelectedCar()
    {
        int idx = Shared.CarDataManager.SelectedCarIndex;
        Vector3 spown = new Vector3(-1, 1, 11);

        GameObject prefab = (idx >= 0 && idx < ingameCarPrefabs.Length)
            ? ingameCarPrefabs[idx]
            : ingameCarPrefabs[0];
        currentCar = Instantiate(prefab, spown, Quaternion.identity);

        Shared.TargetCamera.target = currentCar.gameObject.transform;
    }

    public void AddMoney(int amount)
    {
        money += amount * moneyExchange;
        SaveMoney();
        OnMoneyChanged?.Invoke(money);
        Debug.Log($"돈 획득: +{amount} → 총합 {money}");
    }

    public void SpendMoney(int amount)
    {
        money = Mathf.Max(0, money - amount);
        SaveMoney();
        OnMoneyChanged?.Invoke(money);
        Debug.Log($"돈 사용: -{amount} → 남은 금액 {money}");
    }

    public void ResetMoney()
    {
        money = 0;
        SaveMoney();
        OnMoneyChanged?.Invoke(money);
    }

    private void SaveMoney()
    {
        PlayerPrefs.SetInt(MONEY_KEY, money);
        PlayerPrefs.Save();
    }

    private void LoadMoney()
    {
        money = PlayerPrefs.GetInt(MONEY_KEY, 0);
    }
}
