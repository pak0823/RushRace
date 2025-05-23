using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("스테이지별 맵 프리팹")]
    public GameObject[] stagePrefabs; // 0: Grass, 1: Desert, 2:DownTown
    [Header("맵 생성 위치")]
    public Vector3 mapSpawnPosition = Vector3.zero; // 원하는 위치로 수정 가능

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
            Debug.LogError("잘못된 스테이지 번호");
            return;
        }

        if(stageNum == 2)
        {
            mapSpawnPosition = Vector3.zero; // 월드 정중앙
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
