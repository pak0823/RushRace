// GameManager.cs - °ÔÀÓ ³» ÀÚ±Ý ¹× »óÅÂ °ü¸®

using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    private int money = 0;
    public int Money => money;
    public static event Action<int> OnMoneyChanged;

    private const string MONEY_KEY = "Money";

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

    public void AddMoney(int amount)
    {
        money += amount;
        SaveMoney();
        OnMoneyChanged?.Invoke(money);
        Debug.Log($"µ· È¹µæ: +{amount} ¡æ ÃÑÇÕ {money}");
    }

    public void SpendMoney(int amount)
    {
        money = Mathf.Max(0, money - amount);
        SaveMoney();
        OnMoneyChanged?.Invoke(money);
        Debug.Log($"µ· »ç¿ë: -{amount} ¡æ ³²Àº ±Ý¾× {money}");
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
