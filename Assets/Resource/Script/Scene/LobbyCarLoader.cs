using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCarLoader : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject carPrefab; // �׽�Ʈ��

    void Start()
    {
        GameObject car = Instantiate(carPrefab, spawnPoint.position, spawnPoint.rotation, spawnPoint);
        car.transform.SetParent(spawnPoint);
    }
}
