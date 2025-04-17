using UnityEngine;
using UnityEngine.UI;

public class MoneyDisplayUI : MonoBehaviour
{
    [Header("UI �ؽ�Ʈ ������Ʈ")]
    public Text moneyText;

    private void OnEnable()
    {
        GameManager.OnMoneyChanged += UpdateDisplay;
        UpdateDisplay(Shared.GameManager != null ? Shared.GameManager.Money : 0);
    }

    private void OnDisable()
    {
        GameManager.OnMoneyChanged -= UpdateDisplay;
    }

    private void UpdateDisplay(int value)
    {
        if (moneyText != null)
        {
            moneyText.text = "���� ������: " + value.ToString() + " $";
        }
    }
}
