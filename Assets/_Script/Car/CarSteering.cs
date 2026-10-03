using UnityEngine;

public class CarSteering : MonoBehaviour
{
    public float turnSpeed = 200f;

    [Header("Sprite Settings")]
    [Tooltip("Masukkan komponen SpriteRenderer karakter Anda")]
    public SpriteRenderer carSpriteRenderer;

    [Tooltip("Gambar saat belok kiri")]
    public Sprite leftSprite;

    [Tooltip("Gambar saat belok kanan")]
    public Sprite rightSprite;

    Rigidbody2D rb;

    private Sprite normalSprite;
    private bool isTurning = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (carSpriteRenderer == null)
        {
            carSpriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    public void Steer(float direction)
    {
        float rotationAmount = -direction * turnSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation + rotationAmount);

        if (carSpriteRenderer != null)
        {
            if (Mathf.Abs(direction) > 0.05f)
            {
                if (!isTurning)
                {
                    normalSprite = carSpriteRenderer.sprite;
                    isTurning = true;
                }

                carSpriteRenderer.sprite = (direction < 0) ? leftSprite : rightSprite;
            }
            else 
            {
                if (isTurning)
                {
                    carSpriteRenderer.sprite = normalSprite;
                    isTurning = false;
                }
                else
                {
                    normalSprite = carSpriteRenderer.sprite;
                }
            }
        }
    }
}