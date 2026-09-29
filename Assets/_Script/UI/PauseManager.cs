using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseManager : MonoBehaviour
{
    [Header("References")]
    public CanvasGroup pausePanel;
    public float fadeDuration = 0.25f;

    [Header("Scene Names")]
    public string mainMenuSceneName = "MainMenu";

    private bool isPaused = false;
    private Coroutine fadeRoutine;

    void Start()
    {
        // Pastikan panel pause mati total di awal
        if (pausePanel != null)
        {
            pausePanel.alpha = 0f;
            pausePanel.interactable = false;
            pausePanel.blocksRaycasts = false;
            pausePanel.gameObject.SetActive(false);
        }

        // Pastikan game dimulai dalam keadaan normal
        Time.timeScale = 1f;
    }

    void Update()
    {
        // Tekan ESC untuk Pause / Resume
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        // Jangan pause ulang jika sudah pause
        if (isPaused)
            return;

        isPaused = true;
        Time.timeScale = 0f;

        if (pausePanel == null)
        {
            Debug.LogWarning("PausePanel belum di-assign pada PauseManager.");
            return;
        }

        pausePanel.gameObject.SetActive(true);

        // Hentikan coroutine fade sebelumnya jika ada
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        // Fade In
        fadeRoutine = StartCoroutine(
            FadeCanvasGroup(
                pausePanel,
                0f,
                1f
            )
        );
    }

    public void ResumeGame()
    {
        // Jangan resume jika memang tidak sedang pause
        if (!isPaused)
            return;

        isPaused = false;

        if (pausePanel == null)
        {
            Time.timeScale = 1f;
            return;
        }

        // Hentikan coroutine fade sebelumnya jika ada
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        // Fade Out
        fadeRoutine = StartCoroutine(
            FadeCanvasGroup(
                pausePanel,
                1f,
                0f,
                () =>
                {
                    pausePanel.gameObject.SetActive(false);

                    // Kembalikan waktu game ke normal
                    Time.timeScale = 1f;
                }
            )
        );
    }

    
    public void RestartLevel()
    {
        // Pastikan waktu kembali normal sebelum reload scene
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    
    public void BackToMainMenu()
    {
        // Pastikan waktu kembali normal sebelum pindah scene
        Time.timeScale = 1f;

        SceneManager.LoadScene(mainMenuSceneName);
    }

    private IEnumerator FadeCanvasGroup(
        CanvasGroup cg,
        float from,
        float to,
        System.Action onComplete = null
    )
    {
        float t = 0f;

        // Saat fade-in, panel mulai menerima interaksi
        if (to > from)
        {
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
        else
        {
            // Saat fade-out, panel masih menampilkan fade
            // tetapi tidak bisa diklik
            cg.interactable = false;
            cg.blocksRaycasts = true;
        }

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;

            cg.alpha = Mathf.Lerp(
                from,
                to,
                t / fadeDuration
            );

            yield return null;
        }

        // Pastikan nilai akhirnya tepat
        cg.alpha = to;

        cg.interactable = (to > 0f);
        cg.blocksRaycasts = (to > 0f);

        // Jalankan callback jika ada
        onComplete?.Invoke();
    }
}