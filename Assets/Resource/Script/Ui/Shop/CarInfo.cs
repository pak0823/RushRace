using UnityEngine;
using UnityEngine.UI;

public class CarInfo : MonoBehaviour
{
    [Header("UI 레퍼런스")]
    public Text nameText;
    public Text priceText;
    public Text speedText;
    public Text accelText;
    public Text steerText;

    // 외부에서 호출해서 화면에 표시할 Stats 세팅
    public void SetStats(CarStats stats, int price)
    {
        if (stats == null) return;

        nameText.text = stats.carId;
        priceText.text = $"Price: {price} $";
        speedText.text = $"Top Speed: {stats.maxSpeed:0} km/h";
        accelText.text = $"Accel: {stats.motorTorque:0}";
        steerText.text = $"Steer Angle: {stats.maxSteerAngle:0}°";
    }
}
