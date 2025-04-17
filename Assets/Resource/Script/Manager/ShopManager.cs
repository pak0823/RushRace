using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopManager : MonoBehaviour
{
    [Header("카메라 이동 설정")]
    public Camera mainCamera;
    public Transform[] cameraPositions;  // 인스펙터에 Pos0, Pos1, Pos2 할당
    public float cameraMoveSpeed = 5f;

    [Header("차량 슬롯 & 정보")]
    public Transform[] carSlots;         // CarSlot0, CarSlot1, CarSlot2
    public string[] carNames;            // ["Speedster", "Roadster", "Racer"]
    public int[] carPrices;           // [100, 150, 200]

    [Header("UI 참조")]
    public GameObject infoPanel;         // InfoPanel (처음엔 비활성화)
    public Text carNameText;
    public Text priceText;
    public Button btnBuy;
    public Button btnBack;
    public GameObject selectBtn;

    private Coroutine camCoroutine;
    private int currentIndex;  // OnSelectCar로 세팅된 인덱스

    // 1) 초기 카메라 상태 저장용
    private Vector3 initialCamPos;
    private Quaternion initialCamRot;

    private void Awake()
    {
        // 초기 카메라 위치·회전 저장
        initialCamPos = mainCamera.transform.position;
        initialCamRot = mainCamera.transform.rotation;

        // 버튼 리스너 연결
        //btnBack.onClick.AddListener(() => CloseInfo());
    }

    /// <summary>
    /// 외부에서 호출: UI 버튼에 CarIndex를 지정해 연결
    /// </summary>
    public void OnSelectCar(int index)
    {
        selectBtn.SetActive(false);

        currentIndex = index;
        // 1) 정보 패널 표시
        infoPanel.SetActive(true);
        carNameText.text = carNames[index];
        priceText.text = $"{carPrices[index]} $";

        // 2) 카메라 이동
        if (camCoroutine != null) StopCoroutine(camCoroutine);
        camCoroutine = StartCoroutine(MoveCameraTo(cameraPositions[index]));

        if (Shared.CarDataManager.IsUnlocked(index))
            btnBuy.interactable = false;  // 이미 구매된 차량은 구매 불가
        else
            btnBuy.interactable = Shared.GameManager.Money >= carPrices[index];
    }

    public void OnBuyClicked()
    {
        Debug.Log(Shared.GameManager.Money);
        int price = carPrices[currentIndex];
        if (Shared.GameManager.Money < price) return;  // 안전장치

        // 1) 코인 차감
        Shared.GameManager.SpendMoney(price);
        // 2) 차량 잠금 해제 & 선택
        Shared.CarDataManager.Unlock(currentIndex);
        Shared.CarDataManager.Select(currentIndex);
        // 3) 버튼 상태 업데이트
        btnBuy.interactable = false;

        Debug.Log(carPrices[currentIndex]+ "번째 인덱스" + "구매성공!");
    }

    /// <summary>
    /// 카메라를 target 위치/회전으로 부드럽게 이동시키는 코루틴
    /// </summary>
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

    /// <summary>
    /// 초기 위치로 돌아가는 코루틴
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
    /// 정보 패널 닫기 & 카메라 원위치
    /// </summary>
    public void CloseInfo()
    {
        infoPanel.SetActive(false);

        // 2) 카메라 원위치 이동
        if (camCoroutine != null) StopCoroutine(camCoroutine);
        camCoroutine = StartCoroutine(MoveCameraBack());

        selectBtn.SetActive(true);
    }
}
