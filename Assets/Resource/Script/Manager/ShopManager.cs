using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopManager : MonoBehaviour
{
    [Header("ī�޶� �̵� ����")]
    public Camera mainCamera;
    public Transform[] cameraPositions;
    public float cameraMoveSpeed = 5f;

    [Header("���� ���� & ����")]
    public Transform[] carSlots;       // ���� ������Ʈ
    public CarStats[] carStats;        // CarStats ���µ�
    public int[] carPrices;            // ����

    [Header("UI ����")]
    public GameObject infoPanel;       // InfoPanel ��Ʈ
    public CarInfo carInfo;            // CarInfo ������Ʈ
    public Button btnBuy;
    public Button btnBack;
    public GameObject selectBtn;

    private Coroutine camCoroutine;
    private int currentIndex;

    private Vector3 initialCamPos;
    private Quaternion initialCamRot;

    private void Awake()
    {
        initialCamPos = mainCamera.transform.position;
        initialCamRot = mainCamera.transform.rotation;
        Shared.ShopManager = this;
    }

    private void Start()
    {
        // ����� ���� ���� ��ü�� �� ������
        for (int i = 0; i < carSlots.Length; i++)
        {
            if (Shared.CarDataManager.IsUnlocked(i))
                carSlots[i].gameObject.SetActive(false);
        }
    }

    // ���� ��ư�� �ε��� �����ؼ� ȣ��
    public void OnSelectCar(int index)
    {
        selectBtn.SetActive(false);
        currentIndex = index;

        // 1) GameManager �� ���õ� Stats ����
        Shared.GameManager.selectedStats = carStats[index];

        // 2) InfoPanel ���� stats/price ǥ��
        infoPanel.SetActive(true);
        carInfo.SetStats(carStats[index], carPrices[index]);

        // 3) ī�޶� �̵�
        if (camCoroutine != null) StopCoroutine(camCoroutine);
        camCoroutine = StartCoroutine(MoveCameraTo(cameraPositions[index]));

        // 4) ���� ��ư Ȱ��ȭ ����
        bool unlocked = Shared.CarDataManager.IsUnlocked(index);
        btnBuy.interactable = !unlocked && Shared.GameManager.Money >= carPrices[index];
    }

    // ���� ��ư Ŭ��
    public void OnBuyClicked()
    {
        int price = carPrices[currentIndex];
        if (Shared.GameManager.Money < price) return;

        // ���� ����
        Shared.GameManager.SpendMoney(price);
        // ��� ���� & ����
        Shared.CarDataManager.Unlock(currentIndex);
        Shared.CarDataManager.Select(currentIndex);

        // �ٽ� stats �Ҵ� (�����ϰ�)
        Shared.GameManager.selectedStats = carStats[currentIndex];

        // UI ����
        btnBuy.interactable = false;
        selectBtn.transform.GetChild(currentIndex - 1).gameObject.SetActive(false);
        carSlots[currentIndex].gameObject.SetActive(false);
        CloseInfo();

        Debug.Log($"[{currentIndex}] ���ż���!");
    }

    // InfoPanel �ݰ� ī�޶� ����
    public void CloseInfo()
    {
        infoPanel.SetActive(false);
        if (camCoroutine != null) StopCoroutine(camCoroutine);
        camCoroutine = StartCoroutine(MoveCameraBack());
        selectBtn.SetActive(true);
    }

    // ī�޶� target ��ġ/ȸ������ �ε巴�� �̵���Ű�� �ڷ�ƾ
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

    // �ʱ� ��ġ�� ���ư��� �ڷ�ƾ
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
}
