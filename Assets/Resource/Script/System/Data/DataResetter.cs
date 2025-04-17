using UnityEngine;

public static class DataResetter
{
    /// <summary>
    /// 모든 PlayerPrefs 키를 삭제합니다.
    /// (주의: 볼륨 설정 등 모든 저장값이 사라집니다)
    /// </summary>
    public static void ResetAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("모든 저장 데이터가 초기화되었습니다.");
    }

    /// <summary>
    /// 게임 머니, 스테이지 잠금, 차량 잠금/선택 등 주요 키만 선별 삭제하려면
    /// DeleteKey를 사용하세요.
    /// </summary>
    public static void ResetSelectiveData(int numStages, int numCars)
    {
        // 머니
        PlayerPrefs.DeleteKey("Money");  // GameManager에서 사용 :contentReference[oaicite:0]{index=0}&#8203;:contentReference[oaicite:1]{index=1}

        // 스테이지 잠금
        for (int i = 0; i < numStages; i++)
            PlayerPrefs.DeleteKey("StageUnlocked_" + i);  // StageData에서 사용 :contentReference[oaicite:2]{index=2}&#8203;:contentReference[oaicite:3]{index=3}

        // 차량 잠금
        for (int i = 0; i < numCars; i++)
            PlayerPrefs.DeleteKey("CarUnlocked_" + i);

        // 선택된 차량
        PlayerPrefs.DeleteKey("CarSelected");

        PlayerPrefs.Save();
        Debug.Log("선택된 저장 키만 초기화되었습니다.");
    }
}
