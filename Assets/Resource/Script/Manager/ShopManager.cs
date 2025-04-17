using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopManager : MonoBehaviour
{
    [Header("ī�޶� �̵� ����")]
    public Camera mainCamera;
    public Transform[] cameraPositions;  // �ν����Ϳ� Pos0, Pos1, Pos2 �Ҵ�
    public float cameraMoveSpeed = 5f;

    [Header("���� ���� & ����")]
    public Transform[] carSlots;         // CarSlot0, CarSlot1, CarSlot2
    public string[] carNames;            // ["Speedster", "Roadster", "Racer"]
    public int[] carPrices;           // [100, 150, 200]

    [Header("UI ����")]
    public GameObject infoPanel;         // InfoPanel (ó���� ��Ȱ��ȭ)
    public Text carNameText;
    public Text priceText;
    public Button btnBuy;
    public Button btnBack;
    public GameObject selectBtn;

    private Coroutine camCoroutine;
    private int currentIndex;  // OnSelectCar�� ���õ� �ε���

    // 1) �ʱ� ī�޶� ���� �����
    private Vector3 initialCamPos;
    private Quaternion initialCamRot;

    private void Awake()
    {
        // �ʱ� ī�޶� ��ġ��ȸ�� ����
        initialCamPos = mainCamera.transform.position;
        initialCamRot = mainCamera.transform.rotation;

        // ��ư ������ ����
        //btnBack.onClick.AddListener(() => CloseInfo());
    }

    /// <summary>
    /// �ܺο��� ȣ��: UI ��ư�� CarIndex�� ������ ����
    /// </summary>
    public void OnSelectCar(int index)
    {
        selectBtn.SetActive(false);

        currentIndex = index;
        // 1) ���� �г� ǥ��
        infoPanel.SetActive(true);
        carNameText.text = carNames[index];
        priceText.text = $"{carPrices[index]} $";

        // 2) ī�޶� �̵�
        if (camCoroutine != null) StopCoroutine(camCoroutine);
        camCoroutine = StartCoroutine(MoveCameraTo(cameraPositions[index]));

        if (Shared.CarDataManager.IsUnlocked(index))
            btnBuy.interactable = false;  // �̹� ���ŵ� ������ ���� �Ұ�
        else
            btnBuy.interactable = Shared.GameManager.Money >= carPrices[index];
    }

    public void OnBuyClicked()
    {
        Debug.Log(Shared.GameManager.Money);
        int price = carPrices[currentIndex];
        if (Shared.GameManager.Money < price) return;  // ������ġ

        // 1) ���� ����
        Shared.GameManager.SpendMoney(price);
        // 2) ���� ��� ���� & ����
        Shared.CarDataManager.Unlock(currentIndex);
        Shared.CarDataManager.Select(currentIndex);
        // 3) ��ư ���� ������Ʈ
        btnBuy.interactable = false;

        Debug.Log(carPrices[currentIndex]+ "��° �ε���" + "���ż���!");
    }

    /// <summary>
    /// ī�޶� target ��ġ/ȸ������ �ε巴�� �̵���Ű�� �ڷ�ƾ
    /// </summary>
    private IEnumerator MoveCameraTo(Transform target)
    {
        // �̵� �� ��ġ/ȸ�� ����
        Vector3 startPos = mainCamera.transform.position;
        Quaternion startRot = mainCamera.transform.rotation;
        Vector3 endPos = target.position;
        Quaternion endRot = target.rotation;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * cameraMoveSpeed;
            mainCamera.transform.position = Vector3.Lerp(startPos, endPos, t);
            mainCamera.transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }
    }

    /// <summary>
    /// �ʱ� ��ġ�� ���ư��� �ڷ�ƾ
    /// </summary>
    private IEnumerator MoveCameraBack()
    {
        Vector3 startPos = mainCamera.transform.position;
        Quaternion startRot = mainCamera.transform.rotation;
        Vector3 endPos = initialCamPos;
        Quaternion endRot = initialCamRot;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * cameraMoveSpeed;
            mainCamera.transform.position = Vector3.Lerp(startPos, endPos, t);
            mainCamera.transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }
    }

    /// <summary>
    /// ���� �г� �ݱ� & ī�޶� ����ġ
    /// </summary>
    public void CloseInfo()
    {
        infoPanel.SetActive(false);

        // 2) ī�޶� ����ġ �̵�
        if (camCoroutine != null) StopCoroutine(camCoroutine);
        camCoroutine = StartCoroutine(MoveCameraBack());

        selectBtn.SetActive(true);
    }
}
