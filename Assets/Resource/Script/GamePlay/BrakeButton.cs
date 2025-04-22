using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BrakeButton : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler
{

    void Start()
    {
        if(Application.isMobilePlatform)
        {
            gameObject.SetActive(true);
        }
        else
            gameObject.SetActive(false);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (Shared.Car != null)
            Shared.Car.ForceBraking = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Shared.Car != null)
            Shared.Car.ForceBraking = false;
    }
}
