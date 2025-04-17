using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCarLoader : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject[] carPrefabs;  // 인스펙터에 차량 프리팹 배열 할당

    void Start()
    {
        int idx = Shared.CarDataManager.SelectedCarIndex;
        GameObject prefab = (idx >= 0 && idx < carPrefabs.Length)
            ? carPrefabs[idx]
            : carPrefabs[0];
        GameObject car = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation, spawnPoint);
        car.transform.SetParent(spawnPoint);
    }
}
