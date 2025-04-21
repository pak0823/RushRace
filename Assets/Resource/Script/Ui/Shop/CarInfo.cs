using UnityEngine;
using UnityEngine.UI;

public class CarInfo : MonoBehaviour
{
    [Header("UI ���۷���")]
    public Text nameText;
    public Text priceText;
    public Text speedText;
    public Text accelText;
    public Text steerText;

    // �ܺο��� ȣ���ؼ� ȭ�鿡 ǥ���� Stats ����
    public void SetStats(CarStats stats, int price)
    {
        if (stats == null) return;

        nameText.text = stats.carId;
        priceText.text = $"Price: {price} $";
        speedText.text = $"Top Speed: {stats.maxSpeed:0} km/h";
        accelText.text = $"Accel: {stats.motorTorque:0}";
        steerText.text = $"Steer Angle: {stats.maxSteerAngle:0}��";
    }
}
