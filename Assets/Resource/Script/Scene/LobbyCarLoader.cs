using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCarLoader : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject[] carPrefabs;  // �ν����Ϳ� ���� ������ �迭 �Ҵ�

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
