using UnityEngine;

public class CarDataManager : MonoBehaviour
{
    [Header("차량 잠금 상태")]
    // 예: carUnlocked[0]는 기본 제공 차량(true), 나머지는 false
    public bool[] carUnlocked = new bool[] { true, false, false, false, false, false, false};
    private const string PREF_UNLOCK = "CarUnlocked_";
    private const string PREF_SELECTED = "CarSelected";

    public int SelectedCarIndex { get; private set; }

    private void Awake()
    {
        if (Shared.CarDataManager == null)
        {
            Shared.CarDataManager = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else Destroy(gameObject);
    }

    void LoadData()
    {
        for (int i = 0; i < carUnlocked.Length; i++)
            carUnlocked[i] = PlayerPrefs.GetInt(PREF_UNLOCK + i, carUnlocked[i] ? 1 : 0) == 1;

        SelectedCarIndex = PlayerPrefs.GetInt(PREF_SELECTED, 0);
    }

    void SaveUnlocked(int _idx)
    {
        PlayerPrefs.SetInt(PREF_UNLOCK + _idx, carUnlocked[_idx] ? 1 : 0);
        PlayerPrefs.Save();
    }

    void SaveSelected()
    {
        PlayerPrefs.SetInt(PREF_SELECTED, SelectedCarIndex);
        PlayerPrefs.Save();
    }

    public bool IsUnlocked(int _idx) => _idx >= 0 && _idx < carUnlocked.Length && carUnlocked[_idx];

    public void Unlock(int _idx)
    {
        if (_idx >= 0 && _idx < carUnlocked.Length && !carUnlocked[_idx])
        {
            carUnlocked[_idx] = true;
            SaveUnlocked(_idx);
        }
    }

    public void Select(int _idx)
    {
        if (_idx >= 0 && _idx < carUnlocked.Length && carUnlocked[_idx])
        {
            SelectedCarIndex = _idx;
            SaveSelected();
        }
    }
}
