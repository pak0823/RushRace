// UI_Loading.cs - �ε� ȭ�� ó�� �� ���� �� ǥ��

using UnityEngine;
using UnityEngine.UI;

public class UI_Loading : UIBase
{
    [Header("�ε� ����")]
    public float loadingDuration = 3f;
    private float timer = 0f;
    public Slider loadingSlider;

    [Header("���� ��")]
    public Text tipText;

    public override void Start()
    {
        timer = 0f;
        tipText.text = GetRandomTip();
    }

    public override void Update()
    {
        timer += Time.deltaTime;

        //�����̴� ����� ǥ��
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
                tip = "-���ֿ����� ���� �ؼ��� �ȵ˴ϴ�.-";
                break;
            case 1:
                tip = "-���Ӱ� ������ �ٸ��ϴ�.-";
                break;
            case 2:
                tip = "-���� ���� �����Ͽ� �� ���� �ӵ����� ��⼼��.-";
                break;
            case 3:
                tip = "-������ �Ͽ� �Ƿ��� ������ ������.-";
                break;
            case 4:
                tip = "-�̽��Ϳ��װ� ������ �ִ� ���� �ֽ��ϴ�.-";
                break;
            default:
                tip = "-���׶� �� ����.-";
                break;
        }

        return tip;
    }
}
