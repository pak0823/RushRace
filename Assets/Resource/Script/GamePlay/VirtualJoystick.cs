using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour,
    IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform bgImage;      // 배경
    public RectTransform handleImage;  // 조이스틱 손잡이
    [Range(0.5f, 1f)] public float handleRange = 0.6f;

    Vector2 input = Vector2.zero;
    int pointerId = -1;

    public float Horizontal => input.x;
    public float Vertical => input.y;


    void Start()
    {
        if (Application.isMobilePlatform)
        {
            transform.parent.gameObject.SetActive(true);
        }
        else
        {
            transform.parent.gameObject.SetActive(false);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (pointerId == -1)
            pointerId = eventData.pointerId;
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerId != pointerId) return;

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            bgImage, eventData.position, eventData.pressEventCamera, out pos);

        float radius = bgImage.sizeDelta.x * 0.5f;
        input = (pos / radius);
        input = input.magnitude > 1f ? input.normalized : input;

        handleImage.anchoredPosition = input * radius * handleRange;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId != pointerId) return;
        pointerId = -1;
        input = Vector2.zero;
        handleImage.anchoredPosition = Vector2.zero;
    }
}
