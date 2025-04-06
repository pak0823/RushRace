using UnityEngine;

public class GameManager : MonoBehaviour
{
    public MapManager mapManager;

    void Awake()
    {
        if (Shared.GameManager != null && Shared.GameManager != this)
        {
            Destroy(gameObject);
            return;
        }

        Shared.GameManager = this;
        //DontDestroyOnLoad(this); // �� �̵� �� ����
    }

    void Start()
    {
        mapManager = GetComponent<MapManager>();
        mapManager.LoadStage(StageData.CurrentStageNum);
    }
}