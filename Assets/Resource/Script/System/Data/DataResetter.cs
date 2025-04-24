using UnityEngine;

public static class DataResetter
{
    //모든 PlayerPrefs 키를 삭제
    public static void ResetAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("모든 저장 데이터가 초기화되었습니다.");
    }

    // 게임 머니, 스테이지 잠금, 차량 잠금/선택 등 주요 키만 선별 삭제하려면 DeleteKey를 사용
    public static void ResetSelectiveData(int numStages, int numCars)
    {
        // 머니
        PlayerPrefs.DeleteKey("Money");  // GameManager에서 사용

        // 스테이지 잠금
        for (int i = 0; i < numStages; i++)
            PlayerPrefs.DeleteKey("StageUnlocked_" + i);  // StageData에서

        // 차량 잠금
        for (int i = 0; i < numCars; i++)
            PlayerPrefs.DeleteKey("CarUnlocked_" + i);

        // 선택된 차량
        PlayerPrefs.DeleteKey("CarSelected");

        PlayerPrefs.Save();
        Debug.Log("선택된 저장 키만 초기화되었습니다.");
    }
}
