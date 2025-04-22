// GameManager.cs - ���� �� �ڱ� �� ���� ����

using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int money;
    public int Money => money;
    public static event Action<int> OnMoneyChanged;

    private const string MONEY_KEY = "Money";

    [Header("�ΰ��� ���� ������")]
    public GameObject[] ingameCarPrefabs;
    //public Transform carSpawnPoint;  // ���� ��ġ�� ���� ����Ʈ

    public GameObject currentCar;
    public CarStats selectedStats;     // �÷��̾ ������ SO

    private void Awake()
    {
        if (Shared.GameManager == null)
        {
            Shared.GameManager = this;
            DontDestroyOnLoad(gameObject);
            LoadMoney();

            // �� �ε� �ݹ� ����
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //'eSCENE_INGAME' �� �Ǵ� 'eSCENE_PRACTICE'�� ����
        if (scene.name == eSCENE.eSCENE_INGAME.ToString() || scene.name == eSCENE.eSCENE_PRACTICE.ToString())
        {
            SpawnSelectedCar();
            Debug.Log("�ڵ��� ������ on " + scene.name);
        }
    }
    private void OnDestroy()
    {
        // �����ϰ� �̺�Ʈ ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void SpawnSelectedCar()
    {
        int idx = Shared.CarDataManager.SelectedCarIndex;
        GameObject prefab = (idx >= 0 && idx < ingameCarPrefabs.Length)
            ? ingameCarPrefabs[idx]
            : ingameCarPrefabs[0];
        currentCar = Instantiate(prefab, Vector3.up, Quaternion.identity);
        var stat = currentCar.GetComponent<Car>();
        if(idx > 0)
            stat.stats = selectedStats;
        Shared.TargetCamera.target = currentCar.gameObject.transform;
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
