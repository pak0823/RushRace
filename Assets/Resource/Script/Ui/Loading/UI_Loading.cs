// UI_Loading.cs - 로딩 화면 처리 및 랜덤 팁 표시

using UnityEngine;
using UnityEngine.UI;

public class UI_Loading : UIBase
{
    [Header("로딩 설정")]
    public float loadingDuration = 3f;
    private float timer = 0f;
    public Slider loadingSlider;

    [Header("랜덤 팁")]
    public Text tipText;

    public override void Start()
    {
        timer = 0f;
        tipText.text = GetRandomTip();
    }

    public override void Update()
    {
        timer += Time.deltaTime;

        //슬라이더 진행률 표시
        if (loadingSlider != null)
        {
            float progress = Mathf.Clamp01(timer / loadingDuration);
            loadingSlider.value = progress;
        }

        if (timer >= loadingDuration)
        {
            Shared.SceneMgr.ChangeScene(eSCENE.eSCENE_LOBBY);
        }
    }

    string GetRandomTip()
    {
        string tip;
        int randNum = Random.Range(0, 5);

        switch (randNum)
        {
            case 0:
                tip = "-음주운전은 절대 해서는 안됩니다.-";
                break;
            case 1:
                tip = "-게임과 현실은 다릅니다.-";
                break;
            case 2:
                tip = "-좋은 차를 구입하여 더 높은 속도감을 즐기세요.-";
                break;
            case 3:
                tip = "-연습을 하여 실력을 향상시켜 보세요.-";
                break;
            case 4:
                tip = "-이스터에그가 숨겨져 있는 맵이 있습니다.-";
                break;
            default:
                tip = "-버그라 팁 없음.-";
                break;
        }

        return tip;
    }
}
