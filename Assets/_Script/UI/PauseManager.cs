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
        // Pastikan panel mati total di awal
        pausePanel.alpha = 0f;
        pausePanel.interactable = false;
        pausePanel.blocksRaycasts = false;
        pausePanel.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pausePanel.gameObject.SetActive(true);

        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeCanvasGroup(pausePanel, 0f, 1f));
    }

    public void ResumeGame()
    {
        isPaused = false;

        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeCanvasGroup(pausePanel, 1f, 0f, () =>
        {
            pausePanel.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }));
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float from, float to, System.Action onComplete = null)
    {
        float t = 0f;
        cg.interactable = (to > from); // aktif interaksi di akhir fade-in
        cg.blocksRaycasts = true;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime; // WAJIB unscaled, karena timeScale = 0 saat pause
            cg.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            yield return null;
        }

        cg.alpha = to;
        cg.interactable = (to > 0f);
        cg.blocksRaycasts = (to > 0f);

        onComplete?.Invoke();
    }


}
