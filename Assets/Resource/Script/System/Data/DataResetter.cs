using UnityEngine;

public static class DataResetter
{
    //��� PlayerPrefs Ű�� ����
    public static void ResetAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("��� ���� �����Ͱ� �ʱ�ȭ�Ǿ����ϴ�.");
    }

    // ���� �Ӵ�, �������� ���, ���� ���/���� �� �ֿ� Ű�� ���� �����Ϸ��� DeleteKey�� ���
    public static void ResetSelectiveData(int numStages, int numCars)
    {
        // �Ӵ�
        PlayerPrefs.DeleteKey("Money");  // GameManager���� ���

        // �������� ���
        for (int i = 0; i < numStages; i++)
            PlayerPrefs.DeleteKey("StageUnlocked_" + i);  // StageData����

        // ���� ���
        for (int i = 0; i < numCars; i++)
            PlayerPrefs.DeleteKey("CarUnlocked_" + i);

        // ���õ� ����
        PlayerPrefs.DeleteKey("CarSelected");

        PlayerPrefs.Save();
        Debug.Log("���õ� ���� Ű�� �ʱ�ȭ�Ǿ����ϴ�.");
    }
}
