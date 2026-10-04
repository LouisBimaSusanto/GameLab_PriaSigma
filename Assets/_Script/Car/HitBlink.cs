using System.Collections;
using UnityEngine;

public class HitBlink : MonoBehaviour
{
    [Header("Referensi")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Pengaturan")]
    [SerializeField] private string obstacleTag = "Obstacle";
    [SerializeField] private float blinkDuration = 1f;
    [SerializeField] private float blinkInterval = 0.1f;

    [Header("Mode")]
    [Tooltip("Centang jika memakai material Shader Graph dengan property _FlashAmount")]
    [SerializeField] private bool useWhiteFlash = false;

    [Header("Audio Settings")]
    [Tooltip("Nama SFX yang diputar saat menabrak obstacle")]
    [SerializeField] private string crashSfxName = "crash";

    private Coroutine blinkRoutine;
    private Color originalColor;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        originalColor = spriteRenderer.color;
    }

    private void OnDisable()
    {
        // Pastikan sprite kembali normal kalau objek dinonaktifkan saat berkedip
        if (blinkRoutine != null)
        {
            StopCoroutine(blinkRoutine);
            blinkRoutine = null;
        }
        SetBlink(false);
    }

    // Cek objek itu sendiri, atau parent-nya
    private bool IsObstacle(GameObject obj)
    {
        if (obj.CompareTag(obstacleTag)) return true;
        return obj.transform.parent != null && obj.transform.parent.CompareTag(obstacleTag);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsObstacle(collision.gameObject))
            StartBlink();
    }

    // Pakai ini kalau obstacle kamu Is Trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsObstacle(other.gameObject))
            StartBlink();
    }

    public void StartBlink()
    {
        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);

        if (AudioManager.Instance != null && !string.IsNullOrEmpty(crashSfxName))
        {
            AudioManager.Instance.PlaySFX(crashSfxName);
        }

        blinkRoutine = StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        float elapsed = 0f;
        bool on = false;

        while (elapsed < blinkDuration)
        {
            on = !on;
            SetBlink(on);

            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        SetBlink(false); // kembali normal
        blinkRoutine = null;
    }

    private void SetBlink(bool on)
    {
        if (useWhiteFlash)
        {
            spriteRenderer.material.SetFloat("_FlashAmount", on ? 1f : 0f);
        }
        else
        {
            Color c = originalColor;
            c.a = on ? 0.2f : 1f;
            spriteRenderer.color = c;
        }
    }
}