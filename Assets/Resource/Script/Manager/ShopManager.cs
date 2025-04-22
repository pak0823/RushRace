using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopManager : MonoBehaviour
{
    [Header("카메라 이동 설정")]
    public Camera mainCamera;
    public Transform[] cameraPositions;
    public float cameraMoveSpeed = 5f;

    [Header("차량 슬롯 & 정보")]
    public Transform[] carSlots;       // 슬롯 오브젝트
    public CarStats[] carStats;        // CarStats 에셋들
    public int[] carPrices;            // 가격

    [Header("UI 참조")]
    public GameObject infoPanel;       // InfoPanel 루트
    public CarInfo carInfo;            // CarInfo 컴포넌트
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
        // 언락된 차는 슬롯 자체를 꺼 버린다
        for (int i = 0; i < carSlots.Length; i++)
        {
            if (Shared.CarDataManager.IsUnlocked(i))
                carSlots[i].gameObject.SetActive(false);
        }
    }

    // 슬롯 버튼에 인덱스 지정해서 호출
    public void OnSelectCar(int index)
    {
        selectBtn.SetActive(false);
        currentIndex = index;

        // 1) GameManager 에 선택된 Stats 저장
        Shared.GameManager.selectedStats = carStats[index];

        // 2) InfoPanel 열고 stats/price 표시
        infoPanel.SetActive(true);
        carInfo.SetStats(carStats[index], carPrices[index]);

        // 3) 카메라 이동
        if (camCoroutine != null) StopCoroutine(camCoroutine);
        camCoroutine = StartCoroutine(MoveCameraTo(cameraPositions[index]));

        // 4) 구매 버튼 활성화 여부
        bool unlocked = Shared.CarDataManager.IsUnlocked(index);
        btnBuy.interactable = !unlocked && Shared.GameManager.Money >= carPrices[index];
    }

    // 구매 버튼 클릭
    public void OnBuyClicked()
    {
        int price = carPrices[currentIndex];
        if (Shared.GameManager.Money < price) return;

        // 코인 차감
        Shared.GameManager.SpendMoney(price);
        // 잠금 해제 & 선택
        Shared.CarDataManager.Unlock(currentIndex);
        Shared.CarDataManager.Select(currentIndex);

        // 다시 stats 할당 (안전하게)
        Shared.GameManager.selectedStats = carStats[currentIndex];

        // UI 갱신
        btnBuy.interactable = false;
        selectBtn.transform.GetChild(currentIndex - 1).gameObject.SetActive(false);
        carSlots[currentIndex].gameObject.SetActive(false);
        CloseInfo();

        Debug.Log($"[{currentIndex}] 구매성공!");
    }

    // InfoPanel 닫고 카메라 복귀
    public void CloseInfo()
    {
        infoPanel.SetActive(false);
        if (camCoroutine != null) StopCoroutine(camCoroutine);
        camCoroutine = StartCoroutine(MoveCameraBack());
        selectBtn.SetActive(true);
    }

    // 카메라를 target 위치/회전으로 부드럽게 이동시키는 코루틴
    private IEnumerator MoveCameraTo(Transform target)
    {
        // 이동 전 위치/회전 저장
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

    // 초기 위치로 돌아가는 코루틴
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
