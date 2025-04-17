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
        // 씬 내에 모든 "코인" 태그를 가진 오브젝트를 찾아 리스트에 저장
        GameObject[] coinObjects = GameObject.FindGameObjectsWithTag("Coin");
        coins.AddRange(coinObjects);
    }

    // OnBtnRepeat()가 실행될 때 호출
    public void ResetCoins()
    {
        foreach (var coin in coins)
        {
            // 원래 위치로 리셋할 필요가 있다면, 각 코인의 초기 위치 정보를 미리 저장해두어야 함.
            // 여기선 단순히 재활성화
            coin.SetActive(true);
        }
    }
}
