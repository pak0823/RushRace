using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int money = 0;
    public int Money => money;
    public static event Action<int> OnMoneyChanged; // �̺�Ʈ �߰�

    private const string MONEY_KEY = "Money";

    void Awake()
    {
        if (Shared.GameManager == null)
        {
            Shared.GameManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;
        SaveMoney();
        OnMoneyChanged?.Invoke(money); // �̺�Ʈ �߻�
        Debug.Log($"�� ȹ��: +{amount} �� ���� {money}");
    }

    public void SpendMoney(int amount)
    {
        money = Mathf.Max(0, money - amount);
        SaveMoney();
        OnMoneyChanged?.Invoke(money); // �̺�Ʈ �߻�
        Debug.Log($"�� ���: -{amount} �� ���� �ݾ� {money}");
    }

    public void ResetMoney()
    {
        money = 0;
        SaveMoney();
        OnMoneyChanged?.Invoke(money); // �̺�Ʈ �߻�
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