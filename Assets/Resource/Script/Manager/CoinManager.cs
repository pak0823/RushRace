using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    private List<GameObject> coins = new List<GameObject>();

    private void Awake()
    {
        Shared.CoinManager = this;
    }

    private void Start()
    {
        // �� ���� ��� "����" �±׸� ���� ������Ʈ�� ã�� ����Ʈ�� ����
        GameObject[] coinObjects = GameObject.FindGameObjectsWithTag("Coin");
        coins.AddRange(coinObjects);
    }

    // OnBtnRepeat()�� ����� �� ȣ��
    public void ResetCoins()
    {
        foreach (var coin in coins)
        {
            // ���� ��ġ�� ������ �ʿ䰡 �ִٸ�, �� ������ �ʱ� ��ġ ������ �̸� �����صξ�� ��.
            // ���⼱ �ܼ��� ��Ȱ��ȭ
            coin.SetActive(true);
        }
    }
}
