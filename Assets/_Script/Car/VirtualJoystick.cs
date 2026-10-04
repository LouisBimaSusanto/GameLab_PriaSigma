using UnityEngine;

public class VirtualJoystick : MonoBehaviour
{
    public Vector2 InputDirection { get; private set; }

    [Header("UI Settings")]
    [Tooltip("Masukkan Panel/Canvas pembungkus tombol kiri & kanan di sini")]
    [SerializeField] private GameObject mobileUIContainer;

    private bool isLeftPressed;
    private bool isRightPressed;

    private void Awake()
    {
        // Deteksi otomatis: Sembunyikan UI jika dimainkan di PC/Desktop
        if (mobileUIContainer != null)
        {
            if (SystemInfo.deviceType == DeviceType.Desktop)
            {
                mobileUIContainer.SetActive(false);
            }
            else
            {
                mobileUIContainer.SetActive(true);
            }
        }
    }

    private void Update()
    {
        float x = 0f;

        if (isRightPressed) x = 1f;
        else if (isLeftPressed) x = -1f;

        InputDirection = new Vector2(x, 0f);
    }

    // --- FUNGSI UNTUK TOMBOL KIRI ---
    public void PointerDownLeft() => isLeftPressed = true;
    public void PointerUpLeft() => isLeftPressed = false;

    // --- FUNGSI UNTUK TOMBOL KANAN ---
    public void PointerDownRight() => isRightPressed = true;
    public void PointerUpRight() => isRightPressed = false;
}