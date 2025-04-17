using UnityEngine;

public static class DataResetter
{
    /// <summary>
    /// ��� PlayerPrefs Ű�� �����մϴ�.
    /// (����: ���� ���� �� ��� ���尪�� ������ϴ�)
    /// </summary>
    public static void ResetAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("��� ���� �����Ͱ� �ʱ�ȭ�Ǿ����ϴ�.");
    }

    /// <summary>
    /// ���� �Ӵ�, �������� ���, ���� ���/���� �� �ֿ� Ű�� ���� �����Ϸ���
    /// DeleteKey�� ����ϼ���.
    /// </summary>
    public static void ResetSelectiveData(int numStages, int numCars)
    {
        // �Ӵ�
        PlayerPrefs.DeleteKey("Money");  // GameManager���� ��� :contentReference[oaicite:0]{index=0}&#8203;:contentReference[oaicite:1]{index=1}

        // �������� ���
        for (int i = 0; i < numStages; i++)
            PlayerPrefs.DeleteKey("StageUnlocked_" + i);  // StageData���� ��� :contentReference[oaicite:2]{index=2}&#8203;:contentReference[oaicite:3]{index=3}

        // ���� ���
        for (int i = 0; i < numCars; i++)
            PlayerPrefs.DeleteKey("CarUnlocked_" + i);

        // ���õ� ����
        PlayerPrefs.DeleteKey("CarSelected");

        PlayerPrefs.Save();
        Debug.Log("���õ� ���� Ű�� �ʱ�ȭ�Ǿ����ϴ�.");
    }
}
