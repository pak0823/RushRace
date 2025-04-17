// GameManager.cs - ���� �� �ڱ� �� ���� ����

using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int money = 0;
    public int Money => money;
    public static event Action<int> OnMoneyChanged;

    private const string MONEY_KEY = "Money";

    [Header("�ΰ��� ���� ������")]
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

        // �������� ������ Ȯ�� ��
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
        // ���� currentCar�� Car ������Ʈ�� �پ� �־�� �մϴ�.
    }

    public void AddMoney(int amount)
    {
        money += amount;
        SaveMoney();
        OnMoneyChanged?.Invoke(money);
        Debug.Log($"�� ȹ��: +{amount} �� ���� {money}");
    }

    public void SpendMoney(int amount)
    {
        money = Mathf.Max(0, money - amount);
        SaveMoney();
        OnMoneyChanged?.Invoke(money);
        Debug.Log($"�� ���: -{amount} �� ���� �ݾ� {money}");
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
