using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("���������� �� ������")]
    public GameObject[] stagePrefabs; // 0: Grass, 1: Desert, 2:DownTown
    [Header("�� ���� ��ġ")]
    public Vector3 mapSpawnPosition = Vector3.zero; // ���ϴ� ��ġ�� ���� ����

    private GameObject currentMapInstance;

    void Start()
    {
        Shared.MapManager = this;
        LoadStage(StageData.CurrentStageNum);
    }

    public void LoadStage(int stageNum)
    {
        if (stageNum < 0 || stageNum >= stagePrefabs.Length)
        {
            Debug.LogError("�߸��� �������� ��ȣ");
            return;
        }

        if(stageNum == 2)
        {
            mapSpawnPosition = Vector3.zero; // ���� ���߾�
        }
        else
        {
            mapSpawnPosition = new Vector3(-8, 0, 240);
        }

        if (currentMapInstance != null)
            Destroy(currentMapInstance);

        currentMapInstance = Instantiate(stagePrefabs[stageNum], mapSpawnPosition, Quaternion.identity);
    }
}
