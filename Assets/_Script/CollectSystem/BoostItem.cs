using UnityEngine;
using DG.Tweening; 

public class BoostItem : MonoBehaviour
{
    [Header("Boost Settings")]
    [Tooltip("Persentase penambahan kecepatan dari kecepatan awal (Misal: 5 untuk 5%)")]
    public float boostPercentage = 5f;
    public float boostDuration = 3f;

    [Header("Audio")]
    public string boostSFXName = "speed up";

    [Header("Animation Settings")]
    public float wobbleAngle = 10f;      
    public float wobbleSpeed = 0.5f;     
    public float floatHeight = 0.2f;     

    private void Start()
    {
        transform.localRotation = Quaternion.Euler(0, 0, -wobbleAngle);

        transform.DORotate(new Vector3(0, 0, wobbleAngle), wobbleSpeed)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);

        transform.DOMoveY(transform.position.y + floatHeight, wobbleSpeed * 2f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CarMovement car = collision.GetComponent<CarMovement>();

            if (car != null)
            {
                car.ApplyBoost(boostPercentage, boostDuration);

                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlaySFX(boostSFXName);
                }

                transform.DOKill();

                Destroy(gameObject);
            }
        }
    }
}