using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class LoadingData
{
    public static string TargetScene = "Stage1";
}

public class LoadingScreen : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image progressFill;      // Image type = Filled, Fill Method = Horizontal
    [SerializeField] private TMP_Text progressText;   // opsional, boleh dikosongkan

    [Header("Settings")]
    [SerializeField] private float minLoadingTime = 2f;   // lama loading bar terisi penuh
    [SerializeField] private float holdAtFull = 0.3f;     // jeda sebentar saat 100%

    private IEnumerator Start()
    {
        SetProgress(0f);

        // Tunggu lingkaran transisi masuk ke loading screen selesai dulu
        while (SceneTransition.Instance != null && SceneTransition.Instance.IsTransitioning)
            yield return null;

        float elapsed = 0f;
        while (elapsed < minLoadingTime)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / minLoadingTime));
            SetProgress(t);
            yield return null;
        }

        SetProgress(1f);
        yield return new WaitForSecondsRealtime(holdAtFull);

        // Transisi lingkaran kedua: Loading -> scene tujuan (Stage1)
        SceneTransition.Instance.LoadScene(LoadingData.TargetScene);
    }

    private void SetProgress(float value)
    {
        if (progressFill != null) progressFill.fillAmount = value;
        if (progressText != null) progressText.text = Mathf.RoundToInt(value * 100f) + "%";
    }
}