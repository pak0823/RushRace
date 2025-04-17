// GameManager.cs - 게임 내 자금 및 상태 관리

using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int money = 0;
    public int Money => money;
    public static event Action<int> OnMoneyChanged;

    private const string MONEY_KEY = "Money";

    [Header("인게임 차량 프리팹")]
    public GameObject[] ingameCarPrefabs;

    private GameObject currentCar;

    private void Awake()
    {
        if (Shared.GameManager == null)
        {
            Shared.GameManager = this;
            DontDestroyOnLoad(gameObject);
            LoadMoney();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadMoney();

        // 스테이지 씬인지 확인 후
        if (SceneManager.GetActiveScene().name == "eSCENE_INGAME")
            SpawnSelectedCar();
    }

    void SpawnSelectedCar()
    {
        int idx = Shared.CarDataManager.SelectedCarIndex;
        GameObject prefab = (idx >= 0 && idx < ingameCarPrefabs.Length)
            ? ingameCarPrefabs[idx]
            : ingameCarPrefabs[0];
        currentCar = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        // 이후 currentCar에 Car 컴포넌트가 붙어 있어야 합니다.
    }

    public void AddMoney(int amount)
    {
        money += amount;
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
