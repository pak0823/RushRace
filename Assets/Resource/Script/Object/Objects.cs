// Objects.cs - 코인 등의 회전 아이템과 충돌 처리를 담당

using UnityEngine;

public class Objects : MonoBehaviour
{
    private float rotationSpeed = 100f;
    private float smoothSpeed = 5f;
    private float currentYRotation = 0f;
    private Quaternion targetRotation;

    public SoundData COINGET;

    void Start()
    {
        Shared.Object = this;
        targetRotation = transform.rotation;
    }

    void Update()
    {
        currentYRotation += rotationSpeed * Time.deltaTime;
        targetRotation = Quaternion.Euler(0, currentYRotation, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Shared.SoundManager.PlaySound(COINGET);
            Shared.MissionManager.OnCoinCollected();
            gameObject.SetActive(false);
        }
    }
}
